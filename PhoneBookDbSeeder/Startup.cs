using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneBook.Dal;
using PhoneBook.Dal.Migrations;
using Shared.Dal;
using Npgsql;

namespace PhoneBookDbSeeder
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(HostBuilderContext builderContext)
        {
            if (builderContext == null) throw new ArgumentNullException(nameof(builderContext));

            Configuration = builderContext.Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISystemClock, SystemClock>();
            services.AddSingleton(NpgsqlConnection.GlobalTypeMapper.DefaultNameTranslator);

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PhoneBookDbContext, IPhoneBookMigrationMarker>(connectionString);

            services.AddScoped<PhoneBookDbSeeder>();
        }
    }
}