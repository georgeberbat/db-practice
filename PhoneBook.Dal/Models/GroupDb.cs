using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dex.Ef.Contracts.Entities;

namespace PhoneBook.Dal.Models;

[Table("group")]
[Microsoft.EntityFrameworkCore.Index(nameof(CreatedUtc))]
[Microsoft.EntityFrameworkCore.Index(nameof(DeletedUtc))]
public class GroupDb : ICreatedUtc, IDeletable, IUpdatedUtc
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

    public ICollection<UserDb> Users { get; set; }
}