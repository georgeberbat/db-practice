using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Bll.Models;
using PhoneBook.Bll.Services;

namespace PhoneBook.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserDataService _userDataService;

    public UserController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }

    /// <summary>
    /// Получить всех абонентов с фильтрацией
    /// </summary>
    /// <param name="request">Запрос на фильтрацию</param>
    [HttpGet]
    public async Task<ActionResult<UserShortDataDto[]>> GetUsers([FromQuery] FilterRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _userDataService.GetUsers(request, cancellationToken));
    }

    /// <summary>
    /// Сохранить данные абонента
    /// </summary>
    /// <param name="request">Запрос на сохранение данных абонента</param>
    [HttpPost]
    public async Task<ActionResult<UserData>> Save([FromBody] SaveUserRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _userDataService.Save(request, cancellationToken));
    }

    /// <summary>
    /// Удаление абонента
    /// </summary>
    /// <param name="userId">Идентификатор абонента</param>
    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteUserRequest request, CancellationToken cancellationToken)
    {
        await _userDataService.Delete(request.UserId, cancellationToken);
        return Ok();
    }
}