using Autofac;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SchulIT.UntisExport;
using System.ServiceProcess;
using UntisExportService.Core;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Upload;

namespace UntisExportService.Service
{
    public partial class UntisExportService : ServiceBase
    {
        private IExportService service;

        public UntisExportService()
        {
            InitializeComponent();

            var builder = new ContainerBuilder();

            builder.RegisterType<JsonSettingsService>().As<ISettingsService>();
            builder.RegisterType<Http>().As<IHttp>();
            builder.RegisterType<IccUploadService>().As<IUploadService>();
            builder.RegisterType<ExportService>().As<IExportService>();
            builder.RegisterType<FileSystemWatcherFactory>().As<IFileSystemWatcherFactory>();

            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<SchulIT.UntisExport.UntisExporter>().As<IUntisExporter>();

            var container = builder.Build();

            service = container.Resolve<IExportService>();
        }

        protected override void OnStart(string[] args)
        {
            service.Start();
        }

        protected override void OnStop()
        {
            service.Start();
        }
    }
}
