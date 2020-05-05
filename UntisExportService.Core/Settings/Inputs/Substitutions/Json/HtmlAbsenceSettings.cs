using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Inputs.Substitutions.Json
{
    public class HtmlAbsenceSettings : IHtmlAbsenceSettings
    {
        [JsonProperty("enabled")]
        public bool ParseAbsences { get; set; } = true;

        [JsonProperty("teacher_identifier")]
        public string TeacherIdentifier { get; set; } = "Abwesende Lehrer";

        [JsonProperty("studygroup_identifier")]
        public string StudyGroupIdentifier { get; set; } = "Abwesende Klassen";
    }
}
