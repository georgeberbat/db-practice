using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Bll.Models;
using PhoneBook.Bll.Services;

namespace PhoneBook.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PhoneCategoryController : ControllerBase
{
    private readonly IPhoneCategoryService _categoryService;

    public PhoneCategoryController(IPhoneCategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Получить все категории
    /// </summary>
    /// <param name="request">Дто для фитрации и пагинации</param>
    [HttpGet]
    public async Task<ActionResult<PhoneCategoryDto[]>> Get([FromQuery] FilterRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.Get(request, cancellationToken));
    }

    /// <summary>
    /// Сохранить данные о категории
    /// </summary>
    /// <param name="request">Запрос на сохранение данных о категории</param>
    [HttpPost]
    public async Task<ActionResult<PhoneCategoryDto>> Save([FromBody] SaveCategoryRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _categoryService.Save(request, cancellationToken));
    }

    /// <summary>
    /// Удалить категорию
    /// </summary>
    /// <param name="categoryId">Id категории</param>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid categoryId, CancellationToken cancellationToken)
    {
        await _categoryService.Delete(categoryId, cancellationToken);
        return Ok();
    }
}