using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Json
{
    public class JsonUntisColumnSettings : IUntisColumnSettings
    {
        [JsonProperty("id")]
        public string IdColumn { get; set; } = "Vtr-Nr.";

        [JsonProperty("date")]
        public string DateColumn { get; set; } = "Datum";

        [JsonProperty("lesson")]
        public string LessonColumn { get; set; } = "Stunde";

        [JsonProperty("grades")]
        public string GradesColumn { get; set; } = "(Klasse(n))";

        [JsonProperty("replacement_grades")]
        public string ReplacementGradesColumn { get; set; } = "Klasse(n)";

        [JsonProperty("teachers")]
        public string TeachersColumn { get; set; } = "(Lehrer)";

        [JsonProperty("replacement_teachers")]
        public string ReplacementTeachersColumn { get; set; } = "Vertreter";

        [JsonProperty("subject")]
        public string SubjectColumn { get; set; } = "(Fach)";

        [JsonProperty("replacement_subject")]
        public string ReplacementSubjectColumn { get; set; } = "Fach";

        [JsonProperty("room")]
        public string RoomColumn { get; set; } = "(Raum)";

        [JsonProperty("replacement_room")]
        public string ReplacementRoomColumn { get; set; } = "Raum";

        [JsonProperty("type")]
        public string TypeColumn { get; set; } = "Art";

        [JsonProperty("remark")]
        public string RemarkColumn { get; set; } = "Vertretungs-Text";
    }
}
