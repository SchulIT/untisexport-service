using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using SchulIT.UntisExport.Tuitions.Gpu;
using System.Threading.Tasks;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings.Inputs.Tuitions;

namespace UntisExportService.Core.Inputs.Tuitions
{
    /// <summary>
    /// Watches for new tuitions.
    /// </summary>
    public class TuitionWatcher : GpuWatcherBase<ITuitionInput>
    {
        protected override string Filename { get { return "GPU002.txt"; } }


        private readonly ITuitionExporter exporter;

        public TuitionWatcher(ITuitionExporter exporter, IFileReader fileReader, IFileSystemWatcher watcher, IEventBus eventBus, ILogger<TuitionWatcher> logger)
            : base (fileReader, watcher, eventBus, logger)
        {
            this.exporter = exporter;
        }

        protected override async Task<EventBase> ParseGpuAsync(string gpu)
        {
            var tuitions = await exporter.ParseGpuAsync(gpu, new TuitionExportSettings
            {
                Delimiter = Settings.Delimiter
            });

            return new TuitionEvent(tuitions);
        }
    }
}
