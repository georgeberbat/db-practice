namespace PhoneBook.Bll.Models;

public class UserShortDataDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public GroupDto[] Groups { get; set; }
}