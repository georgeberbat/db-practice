using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Bll.Models;
using PhoneBook.Dal;
using PhoneBook.Dal.Models;
using Shared.Exceptions;

namespace PhoneBook.Bll.Services;

public class GroupService : IGroupService
{
    private readonly PhoneBookDbContext _dbContext;
    private readonly IMapper _mapper;

    public GroupService(PhoneBookDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<GroupDto[]> Get(FilterRequest filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Groups.Where(x => !x.DeletedUtc.HasValue);

        if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{filter.SearchPhrase}%"));
        }

        if (filter.Page > 0 && filter.Size > 0)
        {
            query = query.OrderBy(x => x.Name).Skip(filter.Size.Value * (filter.Page.Value - 1)).Take(filter.Size.Value);
        }

        var groups = await query.ToArrayAsync(cancellationToken);

        return _mapper.Map<GroupDto[]>(groups);
    }

    public async Task<GroupDto> Save(SaveGroupRequest request, CancellationToken cancellationToken)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        if (await GetGroupByName(request.Name, cancellationToken) != null)
        {
            throw new EntityExistException(request.Name, nameof(GroupDb));
        }

        var groupDb = await GetGroupById(request.Id, cancellationToken);

        if (groupDb == null)
        {
            groupDb = _mapper.Map<GroupDb>(request);
            await _dbContext.AddAsync(groupDb, cancellationToken);
        }
        else
        {
            _mapper.Map(request, groupDb);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GroupDto>(await GetGroupById(groupDb.Id, cancellationToken));
    }

    public async Task Delete(Guid groupId, CancellationToken cancellationToken)
    {
        var groupDb = await GetGroupById(groupId, cancellationToken);

        if (groupDb == null) throw new EntityNotFoundException<GroupDb>(groupId.ToString());

        groupDb.DeletedUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<GroupDb?> GetGroupById(Guid? id, CancellationToken cancellationToken)
    {
        return await _dbContext.Groups.SingleOrDefaultAsync(x => x.Id == id && !x.DeletedUtc.HasValue, cancellationToken);
    }

    private async Task<GroupDb?> GetGroupByName(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Groups.SingleOrDefaultAsync(x => x.Name == name && !x.DeletedUtc.HasValue, cancellationToken);
    }
}