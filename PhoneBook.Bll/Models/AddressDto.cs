namespace PhoneBook.Bll.Models;

/// <summary>
/// Дто данных адреса
/// </summary>
public class AddressDto
{
    /// <summary>
    /// Название района/региона
    /// </summary>
    /// <example>Слободзейский район</example>
    public string? Region { get; set; }

    /// <summary>
    /// Название города
    /// </summary>
    /// <example>Слободзея</example>
    public string? City { get; set; }

    /// <summary>
    /// Название улицы
    /// </summary>
    /// <example>Арбат</example>
    public string? Street { get; set; }

    /// <summary>
    /// Номер дома
    /// </summary>
    /// <example>5</example>
    public int? House { get; set; }

    /// <summary>
    /// Номер/название корпуса
    /// </summary>
    /// <example>2</example>
    public string? Block { get; set; }

    /// <summary>
    /// Номер квартиры
    /// </summary>
    /// <example>3</example>
    public int? Flat { get; set; }
}