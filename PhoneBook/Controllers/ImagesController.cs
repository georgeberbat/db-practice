using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Bll.Services;

namespace PhoneBook.Controllers;

[ApiController]
[Route("[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IFileService _fileService;

    public ImagesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Получение картинки
    /// </summary>
    [HttpGet]
    [Route("{imagePath}")]
    public ActionResult<Stream> GetImage(string imagePath, CancellationToken cancellationToken)
    {
        return _fileService.GetImage(imagePath, cancellationToken);
    }
}