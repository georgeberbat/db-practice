using System;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Shared.Dal;

namespace PhoneBook.Dal;

public class PhoneBookDbContextFactory : IDesignTimeDbContextFactory<PhoneBookDbContext>
{
    public PhoneBookDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var optionsBuilder = new DbContextOptionsBuilder<PhoneBookDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new PhoneBookDbContext(new SystemClock(), new ModelStore<PhoneBookDbContext>(),
            NpgsqlConnection.GlobalTypeMapper.DefaultNameTranslator, optionsBuilder.Options);
    }
}