using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SchulIT.UntisExport;
using UntisExportService.Core;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Settings.Json;
using UntisExportService.Core.StudyGroups;
using UntisExportService.Core.Upload;

namespace UntisExportService.WindowsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterAssemblyTypes(typeof(IModelStrategy).Assembly)
                        .Where(x => x.IsAssignableTo<IModelStrategy>())
                        .AsImplementedInterfaces();

                    builder.RegisterType<JsonFileStudyGroupResolver>().As<IStudyGroupResolveStrategy>().SingleInstance();
                    builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();
                    builder.RegisterType<Http>().As<IHttp>().SingleInstance();
                    builder.RegisterType<IccUploadService>().As<IUploadService>().SingleInstance();
                    builder.RegisterType<ExportService>().As<IExportService>().SingleInstance();
                    builder.RegisterType<FileSystemWatcher>().As<IFileSystemWatcher>();

                    builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
                    builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

                    builder.RegisterType<UntisExporter>().As<IUntisExporter>().SingleInstance();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseWindowsService();
    }
}
