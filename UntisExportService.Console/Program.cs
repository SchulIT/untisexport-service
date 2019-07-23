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

            builder.RegisterType<JsonSettingsService>().As<ISettingsService>().SingleInstance();
            builder.RegisterType<Http>().As<IHttp>().SingleInstance();
            builder.RegisterType<IccUploadService>().As<IUploadService>().SingleInstance();
            builder.RegisterType<ExportService>().As<IExportService>().SingleInstance();
            builder.RegisterType<FileSystemWatcher>().As<IFileSystemWatcher>();

            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<UntisExporter>().As<IUntisExporter>().SingleInstance();

            var container = builder.Build();

            var service = container.Resolve<IExportService>();
            service.Start();
            while (true) { }
        }
    }
}
