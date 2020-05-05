﻿using Autofac;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Redbus;
using Redbus.Configuration;
using Redbus.Interfaces;
using SchulIT.IccImport;
using SchulIT.SchildExport;
using SchulIT.UntisExport;
using UntisExportService.Core;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Inputs;
using UntisExportService.Core.Inputs.Exams;
using UntisExportService.Core.Inputs.Substitutions;
using UntisExportService.Core.Inputs.Supervisions;
using UntisExportService.Core.Inputs.Timetable;
using UntisExportService.Core.Inputs.Tuitions;
using UntisExportService.Core.Outputs;
using UntisExportService.Core.Outputs.Icc;
using UntisExportService.Core.Outputs.Json;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Settings.Json;
using UntisExportService.Core.Tuitions;
using UntisExportService.Outputs;

namespace UntisExportService.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ExportService>().As<IExportService>().SingleInstance();

            // Register IO stuff
            builder.RegisterType<FileReader>().As<IFileReader>().SingleInstance();
            builder.RegisterType<FileSystemWatcher>().As<FileSystemWatcher>().InstancePerDependency();

            // Register watchers
            builder.RegisterAssemblyTypes(typeof(IExportService).Assembly)
                .Where(x => x.Name.EndsWith("Watcher"))
                .AsImplementedInterfaces();

            // Register adapters
            builder.RegisterAssemblyTypes(typeof(IExportService).Assembly)
                .Where(x => x.Name.EndsWith("Adapter"))
                .AsImplementedInterfaces();

            // Register outputs
            builder.RegisterType<WeekMappingHelper>().AsSelf().SingleInstance();
            builder.RegisterType<IccOutputHandler>().As<IOutputHandler>().Keyed<IOutputHandler>("icc");
            builder.RegisterType<JsonOutputHandler>().As<IOutputHandler>().Keyed<IOutputHandler>("file");
            builder.RegisterType<Http>().As<IHttp>();

            // Register tuitions
            builder.RegisterType<TuitionResolver>().As<ITuitionResolver>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(ITuitionResolveStrategy).Assembly)
                .Where(x => !x.Name.StartsWith("Abstract") && x.Name.EndsWith("Strategy"))
                .AsImplementedInterfaces();

            // Register settings
            builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();

            // Register logging
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            // Register external dependencies
            builder.Register(c => new EventBusConfiguration { ThrowSubscriberException = true }).As<IEventBusConfiguration>().SingleInstance();
            builder.RegisterType<EventBus>().As<IEventBus>().SingleInstance();
            builder.RegisterType<Exporter>().As<IExporter>().SingleInstance();
            builder.RegisterType<IccImporter>().As<IIccImporter>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(AbstractExporter).Assembly)
                .Where(x => x.Name.EndsWith("Exporter"))
                .AsImplementedInterfaces();

            var container = builder.Build();

            var service = container.Resolve<IExportService>();
            service.Start();
            while (true) { }
        }
    }
}
