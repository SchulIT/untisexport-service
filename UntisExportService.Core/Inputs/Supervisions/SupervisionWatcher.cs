using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using SchulIT.UntisExport.Supervisions.Gpu;
using System.Linq;
using System.Threading.Tasks;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings.Inputs.Supervisions;

namespace UntisExportService.Core.Inputs.Supervisions
{
    public class SupervisionWatcher : GpuWatcherBase<ISupervisionInput>
    {
        protected override string Filename { get { return "GPU009.txt"; } }

        private readonly ISupervisionExporter exporter;

        public SupervisionWatcher(ISupervisionExporter exporter, IFileReader reader, IFileSystemWatcher fileSystemWatcher, IEventBus eventBus, ILogger<SupervisionWatcher> logger)
            : base(reader, fileSystemWatcher, eventBus, logger)
        {
            this.exporter = exporter;
        }

        protected override async Task<EventBase> ParseGpuAsync(string gpu)
        {
            var supervisions = await exporter.ParseGpuAsync(gpu, new SupervisionExportSettings { Delimiter = Settings.Delimiter });
            return new SupervisionEvent(supervisions.ToList());
        }
    }
}
