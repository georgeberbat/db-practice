
namespace PhoneBook.Bll.Models
{
    /// <summary>
    /// Дто для получения данных пользователя
    /// </summary>
    public class UserData
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public PhoneData[] PhoneNumbers { get; set; }
        public string Address { get; set; }
        public GroupDto[] Groups { get; set; }
        public string Email { get; set; } = null!;
        public string? ContactImage { get; set; }
    }
}