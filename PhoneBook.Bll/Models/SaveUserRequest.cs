using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhoneBook.Bll.Models;

/// <summary>
/// Дто сохранения данных пользователя
/// </summary>
public class SaveUserRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Идентификаторы групп, к которым пользователь относится
    /// </summary>
    [Required]
    public Guid[] GroupIds { get; set; }

    /// <summary>
    /// Url фото
    /// </summary>
    [JsonPropertyName("contact_image")]
    public string? ContactImage { get; set; }

    /// <summary>
    /// Мобильные номера пользователя
    /// </summary>
    [Required]
    public SavePhoneNumberDto[] PhoneNumbers { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }
}