using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.External.Schild.Json
{
    public class SchildSettings : ISchildSettings
    {
        [JsonProperty("connection_string")]
        public string ConnectionString { get; set; } = string.Empty;
    }
}
