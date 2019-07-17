using Autofac;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SchulIT.UntisExport;
using UntisExportService.Core;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Upload;

namespace UntisExportService.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<JsonSettingsService>().As<ISettingsService>();
            builder.RegisterType<Http>().As<IHttp>();
            builder.RegisterType<IccUploadService>().As<IUploadService>();
            builder.RegisterType<ExportService>().As<IExportService>();
            builder.RegisterType<FileSystemWatcherFactory>().As<IFileSystemWatcherFactory>();

            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<UntisExporter>().As<IUntisExporter>();

            var container = builder.Build();

            var service = container.Resolve<IExportService>();
            service.Start();
            while (true) { }
        }
    }
}
