using System.Diagnostics;
using Dex.Ef.Contracts;
using Dex.Ef.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace Shared.Dal
{
    public abstract class BaseDbContext<T> : DbContext where T : DbContext
    {
        private static List<Type> RegisteredEnums { get; } = new();

        private readonly ISystemClock _clock;
        private readonly IModelStore _modelStore;
        private readonly INpgsqlNameTranslator _nameTranslator;

        public bool IsReadOnly => ChangeTracker.QueryTrackingBehavior == QueryTrackingBehavior.NoTracking;

        protected BaseDbContext(ISystemClock clock, IModelStore modelStore, INpgsqlNameTranslator nameTranslator, DbContextOptions<T> options)
            : base(options)
        {
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _modelStore = modelStore ?? throw new ArgumentNullException(nameof(modelStore));
            _nameTranslator = nameTranslator ?? throw new ArgumentNullException(nameof(nameTranslator));

            SavingChanges += OnSavingChanges;
        }

        protected static void RegisterEnum<TEnum>() where TEnum : struct, Enum
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<TEnum>();
            RegisteredEnums.Add(typeof(TEnum));
        }

        public async Task AddOrUpdate<TE, TPk>(TE entity, CancellationToken cancellationToken) where TE : class, IEntity<TPk> where TPk : IComparable
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var set = Set<TE>();
            if (await set.AnyAsync(x => x.Id.Equals(entity.Id), cancellationToken))
            {
                set.Update(entity);
            }
            else
            {
                set.Add(entity);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null) throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            // register extensions
            modelBuilder.HasPostgresExtension("uuid-ossp");

            // register enums
            CreateEnumsModels(modelBuilder);

            // register models
            var sw = Stopwatch.StartNew();
            foreach (var modeType in _modelStore.GetModels())
            {
                modelBuilder.Entity(modeType);
            }

            // register advanced
            modelBuilder.NormalizeEmail();
            modelBuilder.SetUtcDateTimeKind();

            Trace.WriteLine("OnModelCreating: " + sw.Elapsed);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

#if DEBUG
#pragma warning disable CA1062
            optionsBuilder.EnableSensitiveDataLogging();
#pragma warning restore CA1062
#endif
        }

        protected virtual Type[] GetIgnoreConcurrencyTokenTypes()
        {
            return Array.Empty<Type>();
        }

        private void CreateEnumsModels(ModelBuilder modelBuilder)
        {
            foreach (var type in RegisteredEnums)
            {
                string name = _nameTranslator.TranslateTypeName(type.Name);
                string[] labels = Enum.GetNames(type).Select(x => _nameTranslator.TranslateMemberName(x)).ToArray();
                modelBuilder.HasPostgresEnum(name, labels);
            }
        }

        private void OnSavingChanges(object? sender, SavingChangesEventArgs e)
        {
            if (IsReadOnly) throw new NotSupportedException("context is readonly");

            var now = _clock.UtcNow;
            foreach (var x in ChangeTracker.Entries())
            {
                if (x.State == EntityState.Added && x.Entity is ICreatedUtc created)
                {
                    if (created.CreatedUtc == default)
                    {
                        created.CreatedUtc = now.UtcDateTime;
                    }
                }

                if (x.State is EntityState.Added or EntityState.Modified && x.Entity is IUpdatedUtc updated)
                {
                    updated.UpdatedUtc = now.UtcDateTime;
                }
            }
        }
    }
}