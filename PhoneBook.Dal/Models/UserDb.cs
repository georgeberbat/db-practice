using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dex.Ef.Contracts.Entities;
using PhoneBook.Dal.Enums;

namespace PhoneBook.Dal.Models;

[Table("user")]
[Microsoft.EntityFrameworkCore.Index(nameof(CreatedUtc))]
[Microsoft.EntityFrameworkCore.Index(nameof(DeletedUtc))]
public class UserDb : ICreatedUtc, IDeletable, IUpdatedUtc
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("created_utc")]
    public DateTime CreatedUtc { get; set; }

    [Column("deleted_utc")]
    public DateTime? DeletedUtc { get; set; }

    [Column("updated_utc")]
    public DateTime UpdatedUtc { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("image_base64")]
    public string? ContactImage { get; set; }

    [Column("gender")]
    public GenderType Gender { get; set; }

    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    public ICollection<GroupDb> Groups { get; set; }
    public ICollection<PhoneDataDb> Phones { get; set; }
}