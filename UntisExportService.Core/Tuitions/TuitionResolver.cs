using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using UntisExportService.Core.Settings;

namespace UntisExportService.Core.Tuitions
{
    public class TuitionResolver : ITuitionResolver
    {
        private readonly IEnumerable<ITuitionResolveStrategy> strategies;
        private readonly ISettingsService settingsService;
        private readonly ILogger<TuitionResolver> logger;

        public TuitionResolver(IEnumerable<ITuitionResolveStrategy> strategies, ISettingsService settingsService, ILogger<TuitionResolver> logger)
        {
            this.strategies = strategies;
            this.settingsService = settingsService;
            this.logger = logger;
        }

        private ITuitionResolveStrategy GetStrategy(Settings.Tuitions.ITuitionResolver settings)
        {
            if(settings == null)
            {
                throw new ArgumentException("No input for tuitions specified.");
            }

            foreach(var strategy in strategies)
            {
                if(strategy.Supports(settings))
                {
                    return strategy;
                }
            }

            logger.LogError($"Found no ITuitionResolveStrategy for type {settings.Type}.");
            return null;
        }

        public void Initialize()
        {
            var settings = settingsService.Settings.Tuition;
            GetStrategy(settings)?.Initialize(settings);
        }

        public string ResolveTuition(string grade, string subject, string teacher)
        {
            var settings = settingsService.Settings.Tuition;
            return GetStrategy(settings)?.ResolveTuition(grade, subject, teacher);
        }


        public string ResolveStudyGroup(string grade)
        {
            var settings = settingsService.Settings.Tuition;
            return GetStrategy(settings)?.ResolveStudyGroup(grade);
        }
    }
}
