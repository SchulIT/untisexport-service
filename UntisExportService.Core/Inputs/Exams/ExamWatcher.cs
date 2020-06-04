using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using SchulIT.UntisExport.Exams;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings.Inputs.Exams;

namespace UntisExportService.Core.Inputs.Exams
{
    public class ExamWatcher : WatcherBase<IExamInput>
    {
        protected override bool CanStart { get { return settings != null; } }

        private IExamInput settings;

        private readonly IEnumerable<IAdapter> adapters;
        private readonly IFileReader fileReader;
        private readonly ILogger<ExamWatcher> logger;

        public ExamWatcher(IEnumerable<IAdapter> adapters, IFileReader fileReader, IFileSystemWatcher watcher, IEventBus eventBus, ILogger<ExamWatcher> logger)
            : base(watcher, eventBus, logger)
        {
            this.adapters = adapters;
            this.fileReader = fileReader;
            this.logger = logger;
        }

        public override void Configure(IExamInput settings)
        {
            this.settings = settings as IHtmlExamInput;
        }

        protected override async Task<IEnumerable<EventBase>> OnFilesChanged()
        {
            var exams = new List<Exam>();

            foreach(var adapter in adapters)
            {
                if(adapter.Use(settings))
                {
                    var files = FilesystemUtils.GetFiles(settings.Path, adapter.SearchPattern);
                    logger.LogDebug($"Found {files.Length} file(s) matching '{adapter.SearchPattern}'.");

                    foreach (var file in files)
                    {
                        logger.LogDebug($"Found file {file}.");
                        var contents = await fileReader.GetContentsAsync(file, Encoding.GetEncoding(settings.Encoding));

                        logger.LogDebug($"File {file} was read. Parsing exams.");

                        var result = await adapter.GetExamsAsync(contents, settings);
                        exams.AddRange(result.Exams);
                        logger.LogDebug($"Got {result.Exams.Count} exam(s).");
                    }
                }
            }

            return FromSingleEvent(new ExamEvent(exams));
        }

        protected override string GetPath()
        {
            return settings.Path;
        }
    }
}
