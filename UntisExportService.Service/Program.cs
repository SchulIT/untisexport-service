using System.ServiceProcess;

namespace UntisExportService.Service
{
    static class Program
    {
        static void Main()
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
