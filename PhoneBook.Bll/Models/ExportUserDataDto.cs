namespace PhoneBook.Bll.Models;

public class ExportUserDataDto
{
    public string Name { get; set; } = null!;
    public string PhoneNumbers { get; set; }
    public string Address { get; set; }
    public string Groups { get; set; }
    public string Email { get; set; } = null!;
}