using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Outputs.Json
{
    public class FileOutput : OutputBase, IFileOutput
    {
        [JsonProperty("type")]
        public override string Type { get; } = "file";

        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;
    }
}
