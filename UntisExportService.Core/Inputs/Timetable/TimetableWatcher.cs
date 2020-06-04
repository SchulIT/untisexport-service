using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using SchulIT.UntisExport.Timetable;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings.Inputs.Timetable;

namespace UntisExportService.Core.Inputs.Timetable
{
    public class TimetableWatcher : WatcherBase<ITimetableInput>
    {

        protected override bool CanStart { get { return settings != null; } }

        private ITimetableInput settings;

        private readonly IEnumerable<IAdapter> adapters;
        private readonly IFileReader fileReader;
        private readonly ILogger<TimetableWatcher> logger;

        public TimetableWatcher(IEnumerable<IAdapter> adapters, IFileReader fileReader, IFileSystemWatcher watcher, IEventBus eventBus, ILogger<TimetableWatcher> logger)
            : base(watcher, eventBus, logger)
        {
            this.adapters = adapters;
            this.fileReader = fileReader;
            this.logger = logger;
        }

        public override void Configure(ITimetableInput settings)
        {
            this.settings = settings;
        }

        protected override string GetPath() => settings.Path;

        protected override async Task<IEnumerable<EventBase>> OnFilesChanged()
        {
            var periodLessons = new Dictionary<string, List<Lesson>>();

            foreach(var adapter in adapters)
            {
                var files = FilesystemUtils.GetFiles(settings.Path, adapter.SearchPattern);
                logger.LogDebug($"{adapter.GetType().ToString()} found {files.Length} file(s).");

                foreach(var file in files)
                {
                    logger.LogDebug($"Found file {file}.");
                    var contents = await fileReader.GetContentsAsync(file, Encoding.GetEncoding(settings.Encoding));

                    logger.LogDebug($"File {file} was read. Parsing timetable.");
                    var result = await adapter.GetLessonsAsync(contents, settings);

                    logger.LogDebug($"Period: {result.Period}, Lessons: {result.Lessons.Count}.");

                    if(adapter.IsMarkedToExport(result.Objective, settings) == false)
                    {
                        logger.LogDebug($"Ignoring timetable for objective '{result.Objective}' as it is not whitelisted in the config.");
                        continue;
                    }

                    if(periodLessons.ContainsKey(result.Period) == false)
                    {
                        periodLessons.Add(result.Period, new List<Lesson>());
                    }

                    periodLessons[result.Period].AddRange(result.Lessons);
                }
            }

            var events = new List<EventBase>();

            foreach(var kv in periodLessons)
            {
                events.Add(new TimetableEvent(kv.Key, kv.Value));

            }

            return events;
        }
    }
}
