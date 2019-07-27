using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings
{
    public class JsonUntisSettings : IUntisSettings
    {
        [JsonProperty("fix_ptags")]
        public bool FixBrokenPTags { get; set; } = true;

        [JsonProperty("datetime_format")]
        public string DateTimeFormat { get; set; } = "d.M.yyyy";

        [JsonProperty("empty_values")]
        public string[] EmptyValues { get; set; } = new string[] { "---", "???" };

        [JsonProperty("include_absentvalues")]
        public bool InlcudeAbsentValues { get; set; } = false;

        [JsonProperty("columns")]
        public IUntisColumnSettings ColumnSettings { get; } = new JsonUntisColumnSettings();

        [JsonProperty("type_replacements")]
        public Dictionary<string, string> TypeReplacements { get; set; }

        [JsonProperty("remove_types")]
        public string[] RemoveSubstitutionsWithTypes { get; set; }
    }
}
