namespace NGC.Logging
{
    using Microsoft.Extensions.Configuration;
    using NGC.Logging.Composition;
    using NGC.Logging.Objects;
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

            var hasDb = !string.IsNullOrEmpty(LogSettings.DbLogMinLevel);
            var hasFile = !string.IsNullOrEmpty(LogSettings.FileLogMinLevel);

            if (hasDb)
                DataAccess.Composition.Setting.ApplyOn(container);

            var target = (hasDb, hasFile) switch
            {
                (true, true) => LogTarget.Both,
                (true, false) => LogTarget.Database,
                _ => LogTarget.File
            };

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
    }
}
