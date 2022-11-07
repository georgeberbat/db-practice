using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Dal
{
    public static class MicrosoftDependencyInjectionExtensions
    {
        public static void AddDbContext<T, TR>(this IServiceCollection services, string connectionString,
            int maxRetryCount = 3, TimeSpan maxRetryDelay = default, bool isReadOnly = false)
            where T : DbContext
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            services.AddDbContext<T>(builder =>
            {
                if (isReadOnly)
                    builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                builder.UseNpgsql(connectionString, optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(typeof(TR).Assembly.GetName().Name);
                    optionsBuilder.EnableRetryOnFailure(maxRetryCount,
                        maxRetryDelay != default ? maxRetryDelay : TimeSpan.FromMilliseconds(500), default);
                });
            });
            services.AddSingleton<IModelStore, ModelStore<T>>();
        }
    }
}