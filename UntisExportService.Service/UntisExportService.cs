using Autofac;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SchulIT.UntisExport;
using System;
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
        }

        internal static IContainer BuildContainer()
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

            return container;
        }

        protected override void OnStart(string[] args)
        {
            var container = BuildContainer();

            service = container.Resolve<IExportService>();

            service.Start();
        }

        protected override void OnStop()
        {
            service?.End();
            service = null;
        }

        internal void TestStartupAndStop(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
            OnStop();
        }
    }
}
