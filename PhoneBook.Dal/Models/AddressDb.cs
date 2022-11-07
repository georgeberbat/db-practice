using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dex.Ef.Contracts.Entities;

namespace PhoneBook.Dal.Models;

[Table("address")]
[Microsoft.EntityFrameworkCore.Index(nameof(CreatedUtc))]
public class AddressDb : ICreatedUtc, IUpdatedUtc
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("created_utc")]
    public DateTime CreatedUtc { get; set; }

    [Column("updated_utc")]
    public DateTime UpdatedUtc { get; set; }

    [Column("region")]
    public string? Region { get; set; }

    [Column("city")]
    public string? City { get; set; }

    [Column("street")]
    public string? Street { get; set; }

    [Column("house")]
    public int? House { get; set; }

    [Column("block")]
    public string? Block { get; set; }

    [Column("flat")]
    public int? Flat { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public UserDb User { get; set; } = null!;
}