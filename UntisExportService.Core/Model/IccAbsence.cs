using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace UntisExportService.Core.Model
{
    public class IccAbsence : IAbsence
    {
        public string Objective { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public IccAbsenceType Type { get; set; }

        public DateTime Date { get; set; }

        public int? LessonStart { get; set; }

        public int? LessonEnd { get; set; }

        public enum IccAbsenceType
        {
            [EnumMember(Value = "study_group")]
            StudyGroup,

            [EnumMember(Value = "teacher")]
            Teacher
        }
    }
}
