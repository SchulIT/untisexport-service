using System;
using System.ServiceProcess;

namespace UntisExportService.Service
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var service = new UntisExportService();
                service.TestStartupAndStop(args);
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
