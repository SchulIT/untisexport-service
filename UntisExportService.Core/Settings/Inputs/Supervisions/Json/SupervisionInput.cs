using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Inputs.Supervisions.Json
{
    public class SupervisionInput : ISupervisionInput
    {
        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("delimiter")]
        public string Delimiter { get; set; } = ",";

        [JsonProperty("encoding")]
        public string Encoding { get; set; } = "iso-8859-1";
    }
}
