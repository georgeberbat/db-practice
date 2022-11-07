using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Dal.Enums;

public enum GenderType
{
    [Display(Name = "Не определено")] None = 0,
    [Display(Name = "Мужской")] Male = 1,
    [Display(Name = "Женский")] Female = 2
}