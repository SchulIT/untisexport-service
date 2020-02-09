using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UntisExportService.Core.Settings;
using UntisExportService.Core.StudyGroups.Json;

namespace UntisExportService.Core.StudyGroups
{
    public class JsonFileStudyGroupResolver : IStudyGroupResolver
    {
        private StudyGroupResolveModel resolveModel;

        private ISettingsService settingsService;
        private ILogger<JsonFileStudyGroupResolver> logger;


        public JsonFileStudyGroupResolver(ISettingsService settingsService, ILogger<JsonFileStudyGroupResolver> logger)
        {
            this.settingsService = settingsService;
            this.logger = logger;
        }

        public void Initialize()
        {
            resolveModel = null;

            if(string.IsNullOrEmpty(settingsService.Settings.StudyGroupsJsonFile))
            {
                logger.LogInformation("study_group_file not provided - resolving study groups will fail.");
                return;
            }

            if(!File.Exists(settingsService.Settings.StudyGroupsJsonFile))
            {
                logger.LogInformation($"Provided study_group_file ({settingsService.Settings.StudyGroupsJsonFile}) does not exist - resolving study groups will fail.");
                return;
            }

            try
            {
                using (var reader = new StreamReader(settingsService.Settings.StudyGroupsJsonFile))
                {
                    resolveModel = JsonConvert.DeserializeObject<StudyGroupResolveModel>(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to parse study_group_file ({settingsService.Settings.StudyGroupsJsonFile}) - resolving study group will fail.");
            }
        }

        public string Resolve(string grade, string subject)
        {
            if(resolveModel == null || resolveModel.StudyGroups == null)
            {
                return null;
            }

            var studyGroups = resolveModel.StudyGroups;

            if (studyGroups.ContainsKey(grade) && studyGroups[grade].ContainsKey(subject))
            {
                return studyGroups[grade][subject];
            }

            logger.LogWarning($"Study group for grade {grade} and subject {subject} was not found.");

            return null;
        }
    }
}
