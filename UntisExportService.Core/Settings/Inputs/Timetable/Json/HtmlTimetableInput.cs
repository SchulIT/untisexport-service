using Newtonsoft.Json;

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
    }
}
