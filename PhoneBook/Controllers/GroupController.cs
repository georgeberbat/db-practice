using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Bll.Models;
using PhoneBook.Bll.Services;

namespace PhoneBook.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    /// <summary>
    /// Получить все группы
    /// </summary>
    /// <param name="request">Дто для фитрации и пагинации</param>
    [HttpGet]
    public async Task<ActionResult<GroupDto[]>> Get([FromQuery] FilterRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _groupService.Get(request, cancellationToken));
    }

    /// <summary>
    /// Сохранить данные о группе
    /// </summary>
    /// <param name="request">Запрос на сохранение данных о группе</param>
    [HttpPost]
    public async Task<ActionResult<GroupDto>> Save([FromBody] SaveGroupRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _groupService.Save(request, cancellationToken));
    }

    /// <summary>
    /// Удалить группу
    /// </summary>
    /// <param name="groupId">Id группы</param>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid groupId, CancellationToken cancellationToken)
    {
        await _groupService.Delete(groupId, cancellationToken);
        return Ok();
    }
}