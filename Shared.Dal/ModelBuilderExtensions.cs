using System.Globalization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shared.Dal
{
    public static class ModelBuilderExtensions
    {
        public static void UseXminAsConcurrencyToken(this ModelBuilder modelBuilder, DbContext dbContext, params Type[]? ignoreTypes)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            var type = dbContext.GetType();

            var dbSets = type.GetProperties()
                .Where(x => x.PropertyType.IsGenericType &&
                            x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(x => x.PropertyType.GetGenericArguments()[0])
                .ToArray();

            foreach (var dbSet in dbSets)
            {
                if (ignoreTypes == null || !ignoreTypes.Contains(dbSet))
                {
                    modelBuilder.Entity(dbSet).UseXminAsConcurrencyToken();
                }
            }
        }

        /// <summary>
        /// Находит все колонки типа <see cref="DateTime"/> в моделях текущего контекста и добавляет им конвертер.
        /// Для хранения в базе дата будет предварительно преобразована в UTC.
        /// При выборке даты из базы, ей будет принудительно устанавлен указанный <paramref name="fetchKind"/>.
        /// </summary>
        /// <remarks>Когда дата хранится в БД как <c>timestamp</c> то у неё теряется изначальный Kind.</remarks>
        /// <param name="modelBuilder"></param>
        public static void SetUtcDateTimeKind(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            UseValueConverterForType<DateTime>(modelBuilder, new DateTimeKindValueConverter(DateTimeKind.Utc, d => d.ToUniversalTime()));
            UseValueConverterForType<DateTime?>(modelBuilder, new DateTimeKindValueConverter(DateTimeKind.Utc, d => d.ToUniversalTime()));
        }

        /// <summary>
        /// Делаем поле с именем Email => ToLower()
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NormalizeEmail(this ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            var emailConverter = new ValueConverter<string, string>(v => v.ToLower(CultureInfo.CurrentCulture), v => v);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType
                    .GetProperties()
                    .Where(p => p.Name == "Email" && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(emailConverter);
                }
            }
        }

        // 
        // ReSharper disable once UnusedMethodReturnValue.Local
        private static ModelBuilder UseValueConverterForType<T>(ModelBuilder modelBuilder, ValueConverter converter)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            return UseValueConverterForType(modelBuilder, typeof(T), converter);
        }

        private static ModelBuilder UseValueConverterForType(ModelBuilder modelBuilder, Type type, ValueConverter converter)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // note that entityType.GetProperties() will throw an exception, so we have to use reflection
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == type);

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion(converter);
                }
            }

            return modelBuilder;
        }

        private sealed class DateTimeKindValueConverter : ValueConverter<DateTime, DateTime>
        {
            public DateTimeKindValueConverter(DateTimeKind kind, Expression<Func<DateTime, DateTime>> convertTo, ConverterMappingHints mappingHints = null!)
                : base(convertTo,
                    dateTime => DateTime.SpecifyKind(dateTime,
                        kind), // timestamp в базе эквивалентен `Unspecified DateTime` поэтому просто восстанавливаем заведомо известный Kind.
                    mappingHints)
            {
            }
        }
    }
}