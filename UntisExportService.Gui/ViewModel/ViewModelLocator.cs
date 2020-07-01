using Autofac;
using GalaSoft.MvvmLight.Messaging;
using NLog.Extensions.Logging;
using UntisExportService.Core;

namespace UntisExportService.Gui.ViewModel
{
    public class ViewModelLocator
    {
        private static IContainer container;

        static ViewModelLocator()
        {
            RegisterServices();
        }

        public static void RegisterServices()
        {
            var builder = new ContainerBuilder();

            ContainerBuilderFactory.RegisterTypes(builder);

            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Messenger>().As<IMessenger>().SingleInstance();

            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<AboutViewModel>().AsSelf().SingleInstance();

            container = builder.Build();
        }

        public IMessenger Messenger { get { return container.Resolve<IMessenger>(); } }

        public MainViewModel Main { get { return container.Resolve<MainViewModel>(); } }

        public AboutViewModel About { get { return container.Resolve<AboutViewModel>(); } }
    }
}
