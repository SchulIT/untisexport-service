using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using SchulIT.UntisExport.Substitutions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings.Inputs.Substitutions;

namespace UntisExportService.Core.Inputs.Substitutions
{
    public class SubstitutionWatcher : WatcherBase<ISubstitutionInput>
    {
        protected override bool CanStart { get { return settings != null; } }

        private ISubstitutionInput settings;

        private readonly IEnumerable<IAdapter> adapters;
        private readonly IFileReader fileReader;
        private readonly ILogger<SubstitutionWatcher> logger;

        public SubstitutionWatcher(IEnumerable<IAdapter> adapters, IFileReader fileReader, IFileSystemWatcher watcher, IEventBus eventBus, ILogger<SubstitutionWatcher> logger)
            : base(watcher, eventBus, logger)
        {
            this.adapters = adapters;
            this.fileReader = fileReader;
            this.logger = logger;
        }

        public override void Configure(ISubstitutionInput settings)
        { 
            this.settings = settings as ISubstitutionInput;
        }

        protected override string GetPath() => settings.Path;

        protected override async Task<IEnumerable<EventBase>> OnFilesChanged()
        {
            var substitutions = new List<Substitution>();
            var infotexts = new List<Infotext>();
            var absences = new List<Absence>();

            foreach(var adapter in adapters)
            {
                if(adapter.Use(settings))
                {
                    var files = Directory.GetFiles(settings.Path, adapter.SearchPattern);
                    logger.LogDebug($"Found {files.Length} file(s) matching '{adapter.SearchPattern}'.");

                    foreach(var file in files)
                    {
                        logger.LogDebug($"Found file {file}.");
                        var contents = await fileReader.GetContentsAsync(file, Encoding.GetEncoding(settings.Encoding));

                        logger.LogDebug($"File {file} was read. Parsing substitutions.");
                        var result = await adapter.GetSubstitutionsAsync(contents, settings);

                        logger.LogDebug($"Got {result.Substitutions.Count} substitution(s).");
                        logger.LogDebug($"Got {result.Absences.Count} absence(s).");
                        logger.LogDebug($"Got {result.Infotexts.Count} infotext(s).");

                        substitutions.AddRange(result.Substitutions);
                        infotexts.AddRange(result.Infotexts);
                        absences.AddRange(result.Absences);
                    }

                    await ReplaceSubstitutionTypesAsync(substitutions);
                    substitutions = await RemoveSubsitutionsWithRemovableTypeAsync(substitutions);
                }
            }

            return new EventBase[]
            {
                new SubstitutionEvent(substitutions),
                new AbsenceEvent(absences),
                new InfotextEvent(infotexts)
            };
        }

        private Task ReplaceSubstitutionTypesAsync(List<Substitution> substitutions)
        {
            logger.LogDebug("Replace substitution types.");

            if (settings.TypeReplacements == null || settings.TypeReplacements.Count == 0)
            {
                logger.LogDebug($"No type replacements given. Skipping.");
                return Task.CompletedTask;
            }

            return Task.Run(() =>
            {
                foreach (var substitution in substitutions)
                {
                    foreach (var kv in settings.TypeReplacements)
                    {
                        substitution.Type = substitution.Type.Replace(kv.Key, kv.Value);
                    }
                }
            });
        }

        private Task<List<Substitution>> RemoveSubsitutionsWithRemovableTypeAsync(List<Substitution> substitutions)
        {
            logger.LogDebug("Remove substitutions with removable types.");

            if (settings.RemoveSubstitutionsWithTypes == null || settings.RemoveSubstitutionsWithTypes.Length == 0)
            {
                logger.LogDebug("No removable types specified. Skipping.");
                return Task.FromResult(substitutions);
            }

            return Task.Run(() =>
            {
                var removableTypes = settings.RemoveSubstitutionsWithTypes;

                return substitutions.Where(x => removableTypes.Contains(x.Type) == false).ToList();
            });
        }
    }
}
