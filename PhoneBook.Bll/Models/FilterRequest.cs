using PhoneBook.Bll.Enums;

namespace PhoneBook.Bll.Models;

/// <summary>
/// Дто для фильтрации и пагинации
/// </summary>
public class FilterRequest
{
    /// <summary>
    /// Фраза для поиска
    /// </summary>
    public string? SearchPhrase { get; set; }

    /// <summary>
    /// По какому полю выполнять поиск
    /// </summary>
    public SearchPlaceType? SearchPlaceType { get; set; }

    /// <summary>
    /// Номер старницы
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Размер страницы
    /// </summary>
    public int? Size { get; set; }
}