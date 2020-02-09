using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.StudyGroups.Json
{
    public class StudyGroupResolveModel
    {
        [JsonProperty("study_groups")]
        public Dictionary<string, Dictionary<string, string>> StudyGroups { get; set; }
    }
}
