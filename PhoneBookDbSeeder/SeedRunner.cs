using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PhoneBookDbSeeder
{
    public static class SeedRunner
    {
        public static async Task Seed(string[] args,
            IConfigurationRoot? externalConfiguration = default)
        {
            Console.WriteLine("Build services...");

            var hostBuilder = Host.CreateDefaultBuilder(args);

            if (externalConfiguration != default)
            {
                hostBuilder.ConfigureAppConfiguration(builder => builder.AddConfiguration(externalConfiguration));
            }

            var host = hostBuilder
                .ConfigureServices((context, collection) =>
                    CreateStartup(context).ConfigureServices(collection))
                .Build();


            Console.WriteLine("Build services completed");

            try
            {
                var env = host.Services.GetRequiredService<IHostEnvironment>();

                Console.WriteLine($"Migrating database: [{env.EnvironmentName}]");

                using var scope = host.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;
                var seeder = serviceProvider.GetRequiredService<PhoneBookDbSeeder>();
                await seeder.RunAsync().ConfigureAwait(false);
                Console.WriteLine("Migrating database completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migrator terminated unexpectedly. Exception: {ex.Message}");
                Environment.ExitCode = -1;
                throw;
            }
        }

        private static Startup CreateStartup(HostBuilderContext context)
        {
            var ctor = typeof(Startup).GetConstructor(new[] { typeof(HostBuilderContext) });

            if (ctor == null)
                throw new InvalidOperationException(
                    "Startup constructor with signature ctor(HostBuilderContext), not found");

            return (Startup) ctor.Invoke(new object[] { context });
        }
    }
}