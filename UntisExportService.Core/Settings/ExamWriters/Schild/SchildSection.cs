using Newtonsoft.Json;
using System;

namespace UntisExportService.Core.Settings.ExamWriters.Schild
{
    public class SchildSection
    {
        [JsonProperty("year")]
        public short SchoolYear { get; set; }

        [JsonProperty("section")]
        public short Section { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }
    }
}
