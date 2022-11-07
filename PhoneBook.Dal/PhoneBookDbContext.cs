using System.Runtime.CompilerServices;
using Dex.Ef.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PhoneBook.Dal.Enums;
using PhoneBook.Dal.Models;
using Shared.Dal;

namespace PhoneBook.Dal
{
    public class PhoneBookDbContext : BaseDbContext<PhoneBookDbContext>
    {
        public PhoneBookDbContext(ISystemClock clock, IModelStore modelStore, INpgsqlNameTranslator nameTranslator,
            DbContextOptions<PhoneBookDbContext> options) : base(clock, modelStore, nameTranslator, options)
        {
        }

        public DbSet<UserDb> Users => Set<UserDb>();
        public DbSet<AddressDb> Addresses => Set<AddressDb>();
        public DbSet<GroupDb> Groups => Set<GroupDb>();
        public DbSet<PhoneCategoryDb> PhoneCategories => Set<PhoneCategoryDb>();
        public DbSet<PhoneDataDb> Phones => Set<PhoneDataDb>();

#pragma warning disable CA2255
        [ModuleInitializer]
#pragma warning restore CA2255
        public static void RegisterEnums()
        {
            RegisterEnum<GenderType>();
        }
    }
}