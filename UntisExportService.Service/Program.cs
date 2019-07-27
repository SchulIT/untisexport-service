using Autofac;
using System;
using System.ServiceProcess;
using UntisExportService.Core.Settings;

namespace UntisExportService.Service
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                if (args.Length != 0 && args[0] == "create-settings")
                {
                    var container = UntisExportService.BuildContainer();
                    container.Resolve<ISettingsService>();
                }
                else
                {
                    var service = new UntisExportService();
                    service.TestStartupAndStop(args);
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new UntisExportService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
