using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Redbus;
using Redbus.Configuration;
using Redbus.Interfaces;
using SchulIT.IccImport;
using SchulIT.SchildExport;
using SchulIT.UntisExport;
using UntisExportService.Core;
using UntisExportService.Core.External.Schild;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Outputs;
using UntisExportService.Core.Outputs.Icc;
using UntisExportService.Core.Outputs.Json;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Settings.Json;
using UntisExportService.Core.Tuitions;
using UntisExportService.Outputs;

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
                    ContainerBuilderFactory.RegisterTypes(builder);

                    builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseWindowsService();
    }
}
