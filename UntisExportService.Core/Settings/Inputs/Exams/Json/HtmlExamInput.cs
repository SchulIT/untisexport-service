using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Inputs.Exams.Json
{
    public class HtmlExamInput : IHtmlExamInput
    {
        [JsonProperty("type")]
        public string Type { get; } = "html";

        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("datetime_format")]
        public string DateTimeFormat { get; set; } = "d.M.yyyy";

        [JsonProperty("encoding")]
        public string Encoding { get; set; } = "iso-8859-1";

        [JsonProperty("columns")]
        public IHtmlExamColumns Columns { get; } = new HtmlExamColumns();
    }
}
