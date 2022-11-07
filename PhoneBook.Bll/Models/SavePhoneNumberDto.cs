namespace PhoneBook.Bll.Models;

public class SavePhoneNumberDto
{
    public string PhoneNumber { get; set; }
    public Guid PhoneCategoryId { get; set; }
}