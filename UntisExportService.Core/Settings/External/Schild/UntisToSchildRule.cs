using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.External.Schild
{
    public class UntisToSchildRule
    {
        [JsonProperty("untis_subject")]
        public string UntisSubject { get; set; }

        [JsonProperty("schild_subject")]
        public string ExternalSubject { get; set; }

        [JsonProperty("is_course")]
        public bool IsCourse { get; set; }

        [JsonProperty("grades")]
        public List<string> Grades { get; set; } = new List<string>();
    }
}
