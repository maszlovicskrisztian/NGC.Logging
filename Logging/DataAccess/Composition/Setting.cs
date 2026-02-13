namespace NGC.Logging.DataAccess.Composition
{
    using NGC.Logging.DataAccess.Repositories;
    using Unity;

    internal static class Setting
    {
        public static IUnityContainer ApplyOn(this IUnityContainer container)
        {
            container.RegisterType<ILogRepository, LogRepository>();
            container.RegisterType<ILogLevelRepository, LogLevelRepository>();
            DatabaseInitializer.SetupDatabase(container);
            return container;
        }
    }
}
