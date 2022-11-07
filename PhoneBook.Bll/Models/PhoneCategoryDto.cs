namespace PhoneBook.Bll.Models;

/// <summary>
/// Дто данных категории
/// </summary>
public class PhoneCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}