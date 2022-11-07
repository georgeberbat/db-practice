using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Bll.Models;
using PhoneBook.Dal;
using PhoneBook.Dal.Models;
using Shared.Exceptions;

namespace PhoneBook.Bll.Services;

public class PhoneCategoryService : IPhoneCategoryService
{
    private readonly PhoneBookDbContext _dbContext;
    private readonly IMapper _mapper;

    public PhoneCategoryService(PhoneBookDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PhoneCategoryDto[]> Get(FilterRequest filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.PhoneCategories.Where(x => !x.DeletedUtc.HasValue);

        if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{filter.SearchPhrase}%"));
        }

        if (filter.Page > 1 && filter.Size > 0)
        {
            query = query.OrderBy(x => x.Name).Skip(filter.Size.Value * (filter.Page.Value - 1)).Take(filter.Size.Value);
        }

        var categories = await query.ToArrayAsync(cancellationToken);

        return _mapper.Map<PhoneCategoryDto[]>(categories);
    }

    public async Task<PhoneCategoryDto> Save(SaveCategoryRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        if (await GetCategoryByName(request.Name, cancellationToken) != null)
        {
            throw new EntityExistException(request.Name, nameof(PhoneCategoryDb));
        }

        var categoryDb = await GetCategoryById(request.Id, cancellationToken);

        if (categoryDb == null)
        {
            categoryDb = _mapper.Map<PhoneCategoryDb>(request);
            await _dbContext.AddAsync(categoryDb, cancellationToken);
        }
        else
        {
            _mapper.Map(request, categoryDb);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PhoneCategoryDto>(await GetCategoryById(categoryDb.Id, cancellationToken));
    }

    public async Task Delete(Guid categoryId, CancellationToken cancellationToken)
    {
        var categoryDb = await GetCategoryById(categoryId, cancellationToken);

        if (categoryDb == null) throw new EntityNotFoundException<GroupDb>(categoryId.ToString());

        categoryDb.DeletedUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<PhoneCategoryDb?> GetCategoryById(Guid? id, CancellationToken cancellationToken)
    {
        return await _dbContext.PhoneCategories.SingleOrDefaultAsync(x => x.Id == id && !x.DeletedUtc.HasValue, cancellationToken);
    }

    private async Task<PhoneCategoryDb?> GetCategoryByName(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.PhoneCategories.SingleOrDefaultAsync(x => x.Name == name && !x.DeletedUtc.HasValue, cancellationToken);
    }
}