using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Inputs.Timetable.Json
{
    public class HtmlTimetableInput : ITimetableInput
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("encoding")]
        public string Encoding { get; set; }

        [JsonProperty("first_lesson")]
        public int FirstLesson { get; set; } = 1;

        [JsonProperty("use_weeks")]
        public bool UseWeeks { get; set; } = true;

        [JsonProperty("subjects")]
        public List<string> Subjects { get; set; } = new List<string>();

        [JsonProperty("grades")]
        public List<string> Grades { get; set; } = new List<string>();

        [JsonProperty("only_last_period")]
        public bool OnlyLastPeriod { get; set; } = false;
    }
}
