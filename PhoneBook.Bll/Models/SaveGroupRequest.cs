namespace PhoneBook.Bll.Models;

/// <summary>
/// Запрос на добавление/обновление данных группы
/// </summary>
public class SaveGroupRequest
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;
}