using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Inputs.Exams.Json
{
    public class GpuExamInput : IGpuExamInput
    {
        [JsonProperty("type")]
        public string Type { get; } = "gpu";

        [JsonProperty("delimiter")]
        public string Delimiter { get; set; } = ",";

        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("encoding")]
        public string Encoding { get; set; } = "iso-8859-1";
    }
}
