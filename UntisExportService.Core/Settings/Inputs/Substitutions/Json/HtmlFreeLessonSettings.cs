using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Inputs.Substitutions.Json
{
    public class HtmlFreeLessonSettings : IHtmlFreeLessonSettings
    {
        [JsonProperty("enabled")]
        public bool ParseFreeLessons { get; set; } = false;

        [JsonProperty("remove")]
        public bool RemoveInfotext { get; set; } = true;

        [JsonProperty("free_lesson_identifier")]
        public string FreeLessonIdentifier { get; set; } = "Unterrichtsfrei";

        [JsonProperty("lesson_identifier")]
        public string LessonIdentifier { get; set; } = "Std.";
    }
}
