using Newtonsoft.Json;
using System.Linq;

namespace UntisExportService.Core.Settings.Json
{
    public class JsonSettings : ISettings
    {
        [JsonProperty("debug")]
        public bool IsDebugModeEnabled { get; set; } = false;

        [JsonProperty("enabled")]
        public bool IsServiceEnabled { get; set; } = true;

        [JsonProperty("html_path")]
        public string HtmlPath { get; set; }

        [JsonProperty("threshold")]
        public int SyncThresholdInSeconds { get; set; } = 2;

        [JsonProperty("encoding")]
        public string Encoding { get; set; } = "iso-8859-1";

        [JsonProperty("endpoint")]
        public IEndpointSettings Endpoint { get; } = new JsonEndpointSettings();

        [JsonProperty("untis")]
        public IUntisSettings Untis { get; } = new JsonUntisSettings();

        [JsonProperty("study_groups_file")]
        public string StudyGroupsJsonFile { get; set; }
    }

    public class JsonEndpointSettings : IEndpointSettings
    {
        [JsonProperty("legacy")]
        public bool UseLegacyStrategy { get; set; } = false;

        [JsonProperty("substitutions")]
        public string SubstitutionsUrl { get; set; }

        [JsonProperty("infotexts")]
        public string InfotextsUrl { get; set; }

        [JsonProperty("absences")]
        public string AbsencesUrl { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
    }

}
