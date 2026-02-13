namespace NGC.Logging.Composition
{
    using Logging.Core;
    using NGC.Logging.Objects;
    using Unity;
    using Unity.Lifetime;

    internal static class Setting
    {
        internal static IUnityContainer ApplyOn(this IUnityContainer container, LogTarget target)
        {
            if (target == LogTarget.Database)
                container.RegisterType<ILogger, DatabaseLogger>();
            else if (target == LogTarget.File)
                container.RegisterType<ILogger, FileLogger>();
            
            return container;
        }
    }
}
