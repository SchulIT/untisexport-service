using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.External.Schild.Json
{
    public class SchildSettings : ISchildSettings
    {
        [JsonProperty("connection_string")]
        public string ConnectionString { get; set; } = string.Empty;

        public string Type => "schild";

        [JsonProperty("grades_with_coursename_as_subject")]
        public List<string> GradesWithCourseNameAsSubject { get; set; } = new List<string>();

        [JsonProperty("subject_conversation_rules")]
        public List<UntisToExternalRule> SubjectConversationRules { get; set; } = new List<UntisToExternalRule>();
    }
}
