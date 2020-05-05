using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using SchulIT.UntisExport.Rooms.Gpu;
using System.Threading.Tasks;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings.Inputs.Rooms;

namespace UntisExportService.Core.Inputs.Rooms
{
    public class RoomWatcher : GpuWatcherBase<IRoomInput>
    {
        protected override string Filename { get { return "GPU005.txt"; } }

        private readonly IRoomExporter exporter;

        public RoomWatcher(IRoomExporter exporter, IFileReader fileReader, IFileSystemWatcher watcher, IEventBus eventBus, ILogger<RoomWatcher> logger)
            : base(fileReader, watcher, eventBus, logger)
        {
            this.exporter = exporter;
        }

        protected override async Task<EventBase> ParseGpuAsync(string gpu)
        {
            var rooms = await exporter.ParseGpuAsync(gpu, new RoomExportSettings { Delimiter = Settings.Delimiter });
            return new RoomEvent(rooms);
        }
    }
}
