using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Inputs.Substitutions.Json
{
    public class HtmlSubstitutionOptions : IHtmlSubstitutionOptions
    {
        [JsonProperty("fix_ptags")]
        public bool FixBrokenPTags { get; set; } = true;

        [JsonProperty("datetime_format")]
        public string DateTimeFormat { get; set; } = "d.M.yyyy";

        [JsonProperty("empty_values")]
        public string[] EmptyValues { get; set; } = new string[] { "---", "???" };

        [JsonProperty("include_absentvalues")]
        public bool InlcudeAbsentValues { get; set; } = false;

        [JsonProperty("type_replacements")]
        public Dictionary<string, string> TypeReplacements { get; set; } = new Dictionary<string, string>();

        [JsonProperty("remove_types")]
        public string[] RemoveSubstitutionsWithTypes { get; set; } = new string[1] { "Klausur" };
    }
}
