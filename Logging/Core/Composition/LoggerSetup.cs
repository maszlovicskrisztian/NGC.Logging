namespace NGC.Logging
{
    using Logging.Core;
    using Microsoft.Extensions.Configuration;
    using NGC.Logging.Composition;
    using Unity;

    public static class LoggerSetup
    {
        public static void ConfigureLogging(IUnityContainer container)
        {
            Configure(container);
        }

        private static void Configure(IUnityContainer container)
        {
            var config = container.Resolve<IConfiguration>();
            FillSettings(config);

            var target = Objects.LogTarget.File;
            if (!string.IsNullOrEmpty(LogSettings.DbLogMinLevel))
            {
                DataAccess.Composition.Setting.ApplyOn(container);
                target = Objects.LogTarget.Database;
            }
            
            Setting.ApplyOn(container, target);
        }

        private static void FillSettings(IConfiguration configuration)
        {
            var logFileLocation = configuration.GetSection("LogFileLocation")?.Value;
            var fileMinLogLevel = configuration.GetSection("LoggingSetting:FileMinLogLevel")?.Value;
            var DbMinLogLevel = configuration.GetSection("LoggingSetting:DbMinLogLevel")?.Value;

            LogSettings.LogFilePath = logFileLocation ?? string.Empty;
            LogSettings.FileLogMinLevel = fileMinLogLevel ?? string.Empty;
            LogSettings.DbLogMinLevel = DbMinLogLevel ?? string.Empty;
        }

        //private static IConfiguration BuildConfiguration()
        //{
        //    return new ConfigurationBuilder()
        //        .SetBasePath(Environment.CurrentDirectory)
        //        .AddJsonFile("logsettings.json", optional: false, reloadOnChange: true)
        //        .Build();
        //}
    }
}
