using Newtonsoft.Json;
using System.Collections.Generic;
using UntisExportService.Core.Settings.ExamWriters;
using UntisExportService.Core.Settings.External;
using UntisExportService.Core.Settings.Outputs;
using UntisExportService.Core.Settings.Tuitions;

namespace UntisExportService.Core.Settings.Json
{
    public class Settings : ISettings
    {
        [JsonProperty("debug")]
        public bool IsDebugModeEnabled { get; set; } = false;

        [JsonProperty("threshold")]
        public int SyncThresholdInSeconds { get; set; } = 2;

        [JsonProperty("inputs")]
        public IInputSettings Inputs { get; set; } = new InputSettings();

        [JsonProperty("outputs")]
        public IList<IOutput> Outputs { get; set; }
        
        [JsonProperty("tuition_resolver")]
        public ITuitionResolver Tuition { get; set; }

        [JsonProperty("external")]
        public IList<IExternal> ExternalServices { get; set; }

        [JsonProperty("exam_writers_resolver")]
        public IExamWritersResolver ExamWriters { get; set; }
    }
}
