using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.ExamWriters.Schild.Json
{
    public class SchildExamWritersResolver : ISchildExamWritersResolver
    {
        [JsonProperty("type")]
        public string Type => "schild";

        [JsonProperty("rules")]
        public List<SchildExamWriterRule> Rules { get; set;  } = new List<SchildExamWriterRule>();

        [JsonProperty("sections")]
        public List<SchildSection> Sections { get; set; } = new List<SchildSection>();
    }
}
