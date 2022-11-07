using Microsoft.AspNetCore.Http;

namespace PhoneBook.Bll.Services;

public interface IFileService
{
    public Task<string> SaveImage(IFormFile formFile, CancellationToken cancellationToken);
    public Task<string> Export(CancellationToken cancellationToken);
    public Stream GetImage(string imageName, CancellationToken cancellationToken);
}