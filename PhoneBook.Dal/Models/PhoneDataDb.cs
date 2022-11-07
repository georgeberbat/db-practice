using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dex.Ef.Contracts.Entities;

namespace PhoneBook.Dal.Models;

[Table("phone_data")]
[Microsoft.EntityFrameworkCore.Index(nameof(CreatedUtc))]
[Microsoft.EntityFrameworkCore.Index(nameof(DeletedUtc))]
public class PhoneDataDb : ICreatedUtc, IDeletable
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("created_utc")]
    public DateTime CreatedUtc { get; set; }

    [Column("deleted_utc")]
    public DateTime? DeletedUtc { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; } = null!;

    [Column("category_id")]
    public Guid CategoryId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public UserDb User { get; set; } = null!;

    [ForeignKey(nameof(CategoryId))]
    public PhoneCategoryDb Category { get; set; } = null!;
}