using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.ExamWriters.Schild
{
    public class SchildExamWriterRule
    {
        [JsonProperty("grades")]
        public List<string> Grades { get; set; }

        [JsonProperty("sections")]
        public List<short> Sections { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }
}
