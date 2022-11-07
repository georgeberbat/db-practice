using PhoneBook.Dal;
using PhoneBook.Dal.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace PhoneBookDbSeeder
{
    public class PhoneBookDbSeeder : IDbSeeder
    {
        private readonly PhoneBookDbContext _dbContext;
        private readonly TimeSpan _delay;

        public PhoneBookDbSeeder(PhoneBookDbContext dbContext, int delaySeconds = 2)
        {
            if (delaySeconds <= 0) throw new ArgumentOutOfRangeException(nameof(delaySeconds));

            _delay = TimeSpan.FromSeconds(Math.Max(1, delaySeconds));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task RunAsync()
        {
            Console.WriteLine($"Seeding database [{typeof(PhoneBookDbContext).FullName}]");

            var repeatCount = 10;
            Exception? lastException = null;

            do
            {
                Console.WriteLine($"Try migrate database [{typeof(PhoneBookDbContext).FullName}]. Repeat: {repeatCount}");

                try
                {
                    await MigrateAsync();
                    await EnsureSeedData();

                    Console.WriteLine($"Migrate database [{typeof(PhoneBookDbContext).FullName}] completed");
                    break;
                }
                catch (Exception ex) when (IsTransientException(ex))
                {
                    lastException = ex;
                    await Task.Delay(_delay);
                }
            } while (--repeatCount > 0);

            if (lastException != null)
            {
                throw new InvalidOperationException($"Migrate database [{typeof(PhoneBookDbContext).FullName}] failed.",
                    lastException);
            }
        }

        private bool IsTransientException(Exception exception)
        {
            return exception is NpgsqlException {IsTransient: true};
        }

        private async Task MigrateAsync()
        {
            if (_dbContext is null)
            {
                throw new ArgumentNullException(nameof(DbContext));
            }

            await _dbContext.Database.MigrateAsync();

            if (_dbContext.Database.GetDbConnection() is NpgsqlConnection npg)
            {
                await npg.OpenAsync();
                npg.ReloadTypes();
            }
        }

        private async Task EnsureSeedData()
        {
            if (_dbContext == null) throw new ArgumentNullException(nameof(DbContext));

            await SeedData();

            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedData()
        {
            if (!await _dbContext.Groups.AnyAsync())
            {
                await _dbContext.Groups.AddRangeAsync(new[]
                {
                    new GroupDb {Name = "Family"},
                    new GroupDb {Name = "Work"},
                    new GroupDb {Name = "Friends"},
                    new GroupDb {Name = "Common"}
                });
            }

            if (!await _dbContext.PhoneCategories.AnyAsync())
            {
                await _dbContext.PhoneCategories.AddRangeAsync(new[]
                {
                    new PhoneCategoryDb {Name = "Home"},
                    new PhoneCategoryDb {Name = "Work"},
                    new PhoneCategoryDb {Name = "Mobile"},
                });
            }
        }
    }
}