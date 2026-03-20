namespace NGC.Logging.Composition
{
    using NGC.Logging.Core;
    using NGC.Logging.Objects;
    using Unity;

    internal static class Setting
    {
        internal static IUnityContainer ApplyOn(this IUnityContainer container, LogTarget target)
        {
            switch (target)
            {
                case LogTarget.Database:
                    container.RegisterType<ILogger, DatabaseLogger>();
                    break;

                case LogTarget.File:
                    container.RegisterType<ILogger, FileLogger>();
                    break;

                case LogTarget.Both:
                    container.RegisterType<ILogger, DatabaseLogger>("database");
                    container.RegisterType<ILogger, FileLogger>("file");
                    container.RegisterFactory<ILogger>(
                        c => new CompositeLogger(c.ResolveAll<ILogger>()));
                    break;
            }

            return container;
        }
    }
}
