using PhoneBook.Bll.Models;

namespace PhoneBook.Bll.Services;

/// <summary>
/// Интерфейс для управления группами
/// </summary>
public interface IGroupService
{
    /// <summary>
    /// Получить все группы
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    public Task<GroupDto[]> Get(FilterRequest filter, CancellationToken cancellationToken);

    /// <summary>
    /// Сохранить данные о группе
    /// </summary>
    /// <param name="request">Запрос на сохранение данных о группе</param>
    public Task<GroupDto> Save(SaveGroupRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить группу
    /// </summary>
    /// <param name="groupId">Id группы</param>
    public Task Delete(Guid groupId, CancellationToken cancellationToken);
}