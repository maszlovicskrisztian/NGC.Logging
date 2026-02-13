namespace NGC.Logging.DataAccess.Composition
{
    using NGC.Logging.DataAccess.Context;
    using NGC.Logging.DataAccess.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Unity;

    /// <summary>
    /// Helper class for database initialization and seeding
    /// </summary>
    internal static class DatabaseInitializer
    {
        private const string DefaultConnectionStringName = "DefaultConnection";
        private const string ErrorKeyword = "ERROR";
        private const string WarningKeyword = "WARNING";
        private const string InfoKeyword = "INFO";

        public static void SetupDatabase(IUnityContainer container)
        {
            // Create DbContext options
            var configuration = container.Resolve<IConfiguration>();
            var connectionString = configuration.GetSection(DefaultConnectionStringName).Value;
            var optionsBuilder = new DbContextOptionsBuilder<LoggingDbContext>();

            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(LoggingDbContext).Assembly.FullName);
            });

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
#endif

            // Register DbContext with Unity using factory
            container.RegisterFactory<LoggingDbContext>(
                c => new LoggingDbContext(optionsBuilder.Options));

            // Initialize database
            InitializeDatabase(optionsBuilder.Options);
        }

        private static void InitializeDatabase(DbContextOptions<LoggingDbContext> options)
        {
            try
            {
                using var context = new LoggingDbContext(options);
                context.Database.EnsureCreated();
                ValidateModel(context);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void ValidateModel(LoggingDbContext context)
        {
            var entityTypes = context.Model.GetEntityTypes();
            if (!entityTypes.Any(e => e.ClrType == typeof(LogLevel)))
            {
                throw new InvalidOperationException("LogLevel entity is not part of the model. Ensure it is defined in the DbContext.");
            }
            
            if (!entityTypes.Any(e => e.ClrType == typeof(Log)))
            {
                throw new InvalidOperationException("Log entity is not part of the model. Ensure it is defined in the DbContext.");
            }

            if (!context.LogLevels.Any(x => x.Keyword == ErrorKeyword) ||
                !context.LogLevels.Any(x => x.Keyword == WarningKeyword) ||
                !context.LogLevels.Any(x => x.Keyword == InfoKeyword))
            {
                throw new InvalidOperationException("LogLevels table does not contain required keywords (Error, Warning, Info). Ensure that the database is seeded with these log levels.");
            }
        }
    }
}