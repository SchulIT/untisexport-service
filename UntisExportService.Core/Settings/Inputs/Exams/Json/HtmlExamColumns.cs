using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Inputs.Exams.Json
{
    public class HtmlExamColumns : IHtmlExamColumns
    {
        [JsonProperty("date")]
        public string DateColumn { get; set; } = "Datum";

        [JsonProperty("lesson_start")]
        public string LessonStartColumn { get; set; } = "Von";

        [JsonProperty("lesson_end")]
        public string LessonEndColumn { get; set; } = "Bis";

        [JsonProperty("grades")]
        public string GradesColumn { get; set; } = "Klassen";

        [JsonProperty("grade_seperator")]
        public char GradesSeparator { get; set; } = ',';

        [JsonProperty("courses")]
        public string CoursesColumn { get; set; } = "Kurs";

        [JsonProperty("courses_separator")]
        public char CoursesSeparator { get; set; } = ',';

        [JsonProperty("teachers")]
        public string TeachersColumn { get; set; } = "Lehrer";

        [JsonProperty("teachers_separator")]
        public char TeachersSeparator { get; set; } = '-';

        [JsonProperty("rooms")]
        public string RoomsColumn { get; set; } = "Räume";

        [JsonProperty("rooms_separator")]
        public char RoomsSeparator { get; set; } = '-';

        [JsonProperty("name")]
        public string NameColumn { get; set; } = "Name";

        [JsonProperty("remark")]
        public string RemarkColumn { get; set; } = "Text";
    }
}
