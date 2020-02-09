using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UntisExportService.Core;

namespace UntisExportService.WindowsService
{
    public class Worker : BackgroundService
    {
        private readonly IExportService exportService;
        private readonly ILogger<Worker> logger;

        public Worker(IExportService exportService, ILogger<Worker> logger)
        {
            this.exportService = exportService;
            this.logger = logger;
        }
        
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            exportService.Start();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            exportService.End();

            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Simply idle around as the service listens for file changes in the background...

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
