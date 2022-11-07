using PhoneBook.Bll.Models;

namespace PhoneBook.Bll.Services;

/// <summary>
/// Интерфейс для управления категориями
/// </summary>
public interface IPhoneCategoryService
{
    /// <summary>
    /// Получить все категории
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    public Task<PhoneCategoryDto[]> Get(FilterRequest filter, CancellationToken cancellationToken);

    /// <summary>
    /// Сохранить данные о категории
    /// </summary>
    /// <param name="request">Запрос на сохранение данных о категории</param>
    public Task<PhoneCategoryDto> Save(SaveCategoryRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить категорию
    /// </summary>
    /// <param name="categoryId">Id категории</param>
    public Task Delete(Guid categoryId, CancellationToken cancellationToken);
}