using DotNet.Globbing;
using Microsoft.Extensions.Logging;
using NaturalSort.Extension;
using Redbus.Events;
using Redbus.Interfaces;
using SchulIT.UntisExport.Timetable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

                if (settings.OnlyLastPeriod)
                {
                    logger.LogDebug("Removing all files but the most recent period.");
                    files = FilterLastPeriod(files, settings.Path);
                    logger.LogDebug($"Still got {files.Length} file(s).");
                }

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

        /// <summary>
        /// Returns a filtered list of files. It only returns all files that belong to the most recent
        /// period. The period is supposed to be part of the path an HTML-file (any directory must have 
        /// a Px-pattern (x meaning a number). 
        /// </summary>
        /// <param name="files">List of absolutes paths of all files.</param>
        /// <param name="path">Path of the root directory for timetable exports.</param>
        /// <returns></returns>
        private string[] FilterLastPeriod(string[] files, string path)
        {
            if(files == null || files.Length == 0)
            {
                return files;
            }

            var sortedFiles = files
                .Select(x => Path.GetRelativePath(path, x))
                .OrderByDescending(x => x, StringComparison.OrdinalIgnoreCase.WithNaturalSort());
            var mostDescendingFile = sortedFiles.FirstOrDefault();

            var parts = mostDescendingFile.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar); // This may break on other OS than Windows
            string periodDirectory = null;

            var regexpDetectPeriodDirectory = new Regex("^P([0-9]+)$");

            foreach (var part in parts)
            { 
                if(regexpDetectPeriodDirectory.IsMatch(part))
                {
                    periodDirectory = part;
                    break;
                }
            }

            if(periodDirectory == null)
            {
                logger.LogError($"Did not find any period directory (e.g. P10, P2, ...) in the most descending file {mostDescendingFile}.");
            }

            logger.LogDebug($"{periodDirectory} seems to be the most recent period.");

            var filesInPeriod = new List<string>();

            var globDetectCurrentPeriod = Glob.Parse($"**/{periodDirectory}/**/*");

            foreach (var file in sortedFiles)
            {
                if(globDetectCurrentPeriod.IsMatch(file))
                {
                    filesInPeriod.Add(Path.Combine(path, file));
                }
            }

            return filesInPeriod.ToArray();
        }
    }
}
