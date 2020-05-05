using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Outputs.Json
{
    public class EndpointSettings : IEndpointSettings
    {
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("response_path")]
        public string ResponsePath { get; set; } = null;
    }
}
