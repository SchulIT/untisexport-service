using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UntisExportService.Core.Model
{
    public class IccSubstitution : ISubstitution
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("lesson_start")]
        public int LessonStart { get; set; }

        [JsonProperty("lesson_end")]
        public int LessonEnd { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("replacement_subject")]
        public string ReplacementSubject { get; set; }

        [JsonProperty("room")]
        public string Room { get; set; }

        [JsonProperty("replacement_room")]
        public string ReplacementRoom { get; set; }

        [JsonProperty("teachers")]
        public ICollection<string> Teachers { get; set; }

        [JsonProperty("replacement_teachers")]
        public ICollection<string> ReplacementTeachers { get; set; }

        [JsonProperty("grades")]
        public ICollection<string> Grades { get; set; }

        [JsonProperty("replacement_grades")]
        public ICollection<string> ReplacementGrades { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("is_supervision")]
        public bool IsSupervision { get; set; } = false;
    }
}
