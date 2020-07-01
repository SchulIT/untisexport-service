using Autofac;
using NLog.Extensions.Logging;
using UntisExportService.Core;

namespace UntisExportService.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            ContainerBuilderFactory.RegisterTypes(builder);
            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            var container = builder.Build();

            var service = container.Resolve<IExportService>();
            service.Start();
            while (true) { }
        }
    }
}
