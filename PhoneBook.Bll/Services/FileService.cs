using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhoneBook.Bll.Models;
using PhoneBook.Dal;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Services;
using FileOptions = PhoneBook.Bll.Options.FileOptions;

namespace PhoneBook.Bll.Services;

public class FileService : IFileService
{
    private readonly PhoneBookDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDataProvider _dataProvider;
    private readonly FileOptions _fileOptions;

    public FileService(PhoneBookDbContext dbContext, IMapper mapper, IDataProvider dataProvider, IOptions<FileOptions> fileOptions)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dataProvider = dataProvider;
        _fileOptions = fileOptions.Value;
    }

    public async Task<string> SaveImage(IFormFile formFile, CancellationToken cancellationToken)
    {
        if (formFile == null) throw new ArgumentNullException(nameof(formFile));

        var fileExtension = Path.GetExtension(formFile.FileName);

        if (!FileExtensions.ImageExtensions.Contains(fileExtension)) throw new FileBadFormatException(fileExtension);

        var fileName = Guid.NewGuid() + fileExtension;
        var filePath = GetPath(_fileOptions.AbsoluteImgPath, fileName);

        await using Stream fileStream = new FileStream(filePath, FileMode.Create);

        await formFile.CopyToAsync(fileStream, cancellationToken);
        return Path.Combine(_fileOptions.AbsoluteImgPath, fileName);
    }

    public async Task<string> Export(CancellationToken cancellationToken)
    {
        var userDbs = await _dbContext.Users.Where(x => !x.DeletedUtc.HasValue)
            .Include(x => x.Phones)
            .Include(x => x.Groups)
            // .Include(x => x.Address)
            .ToArrayAsync(cancellationToken);

        var exportUsers = _mapper.Map<ExportUserDataDto[]>(userDbs);
        var fileName = "Export" + DateTime.Now.ToFileTime() + ".xlsx";
        var filePath = GetPath(_fileOptions.AbsoluteFilePath, fileName);

        await _dataProvider.SaveToExelFile(exportUsers, filePath);
        return filePath;
    }

    public Stream GetImage(string imageName, CancellationToken cancellationToken)
    {
        var imageStream = new FileStream(GetPath(_fileOptions.AbsoluteImgPath, imageName), FileMode.Open);
        return imageStream;
    }

    private static string GetPath(string absolutePath, string fileName)
    {
        return Path.GetFullPath(@"..\") + absolutePath.Replace('/', '\\') + fileName;
    }
}