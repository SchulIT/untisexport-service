using Newtonsoft.Json;

namespace UntisExportService.Core.Settings
{
    public class JsonSettings : ISettings
    {
        [JsonProperty("debug")]
        public bool IsDebugModeEnabled { get; set; } = false;

        [JsonProperty("enabled")]
        public bool IsServiceEnabled { get; set; } = true;

        [JsonProperty("html_path")]
        public string HtmlPath { get; set; }

        [JsonProperty("endpoint")]
        public IEndpointSettings Endpoint { get; } = new JsonEndpointSettings();

        [JsonProperty("untis")]
        public IUntisSettings Untis { get; } = new JsonUntisSettings();
    }

    public class JsonEndpointSettings : IEndpointSettings
    {
        [JsonProperty("substitutions")]
        public string SubstitutionsUrl { get; set; }

        [JsonProperty("infotexts")]
        public string InfotextsUrl { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("new_version")]
        public bool UseNewVersion { get; set; } = true;
        
    }

}
