namespace PhoneBook.Bll.Models;

/// <summary>
/// Дто данных номера телефона
/// </summary>
public class PhoneData
{
    public string PhoneNumber { get; set; } = null!;
    public PhoneCategoryDto Category { get; set; }
}