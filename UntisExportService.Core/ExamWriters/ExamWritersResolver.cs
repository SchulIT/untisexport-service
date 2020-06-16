using Microsoft.Extensions.Logging;
using SchulIT.UntisExport.Exams;
using System;
using System.Collections.Generic;
using System.Text;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Settings.ExamWriters;

namespace UntisExportService.Core.ExamWriters
{
    public class ExamWritersResolver : IExamWritersResolver
    {

        private readonly IEnumerable<IExamWritersResolveStrategy> strategies;
        private readonly ISettingsService settingsService;
        private readonly ILogger<ExamWritersResolver> logger;

        public ExamWritersResolver(IEnumerable<IExamWritersResolveStrategy> strategies, ISettingsService settingsService, ILogger<ExamWritersResolver> logger)
        {
            this.strategies = strategies;
            this.settingsService = settingsService;
            this.logger = logger;
        }

        private IExamWritersResolveStrategy GetStrategy(Settings.ExamWriters.IExamWritersResolver settings)
        {
            if(settings == null)
            {
                throw new ArgumentNullException("settings must not be null.");
            }

            foreach(var strategy in strategies)
            {
                if(strategy.Supports(settings))
                {
                    return strategy;
                }
            }

            logger.LogError($"Found no IExamWritersResolveStrategy for type {settings.Type}.");
            return null;
        }

        public void Initialize()
        {
            var settings = settingsService.Settings.ExamWriters;
            GetStrategy(settings)?.Initialize(settings);
        }

        public List<string> ResolveStudents(string tuition, Exam exam)
        {
            var settings = settingsService.Settings.ExamWriters;
            return GetStrategy(settings)?.Resolve(tuition, exam);
        }
    }
}
