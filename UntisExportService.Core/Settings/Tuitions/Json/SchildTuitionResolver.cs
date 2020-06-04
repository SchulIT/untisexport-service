using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Tuitions.Json
{
    public class SchildTuitionResolver : ISchildTuitionResolver
    {
        [JsonProperty("type")]
        public string Type { get; } = "schild";

        [JsonProperty("grades_with_coursename_as_subject")]
        public List<string> GradesWithCourseNameAsSubject { get; set; } = new List<string>();

        [JsonProperty("subject_conversation_rules")]
        public List<UntisToExternalRule> SubjectConversationRules { get; set; } = new List<UntisToExternalRule>();
    }
}
