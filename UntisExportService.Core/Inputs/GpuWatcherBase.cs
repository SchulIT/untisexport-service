using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings.Inputs;

namespace UntisExportService.Core.Inputs
{
    /// <summary>
    /// A base watcher suitable for GPU-only exports (such as rooms, tuitions and rooms).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GpuWatcherBase<T> : WatcherBase<T>
        where T : IGpuInput
    {
        protected override bool CanStart { get { return Settings != null; } }

        abstract protected string Filename { get; }

        protected T Settings { get; private set; }

        private readonly IFileReader fileReader;
        private readonly ILogger logger;

        protected GpuWatcherBase(IFileReader fileReader, IFileSystemWatcher fileSystemWatcher, IEventBus eventBus, ILogger logger)
            : base(fileSystemWatcher, eventBus, logger)
        {
            this.fileReader = fileReader;
            this.logger = logger;
        }

        public override void Configure(T settings)
        {
            Settings = settings;
        }

        protected override string GetPath() => Settings.Path;

        protected override async Task<IEnumerable<EventBase>> OnFilesChanged()
        {
            var file = Path.Combine(Settings.Path, Filename);

            if (!File.Exists(file))
            {
                logger.LogError($"File {file} does not exist. Skipping.");
                return null;
            }

            var content = await fileReader.GetContentsAsync(file, Encoding.GetEncoding(Settings.Encoding));
            return FromSingleEvent(await ParseGpuAsync(content));
        }

        protected abstract Task<EventBase> ParseGpuAsync(string gpu);
    }
}
