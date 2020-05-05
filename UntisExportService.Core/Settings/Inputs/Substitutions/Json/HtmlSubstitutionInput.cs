using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Inputs.Substitutions.Json
{
    public class HtmlSubstitutionInput : IHtmlSubstitutionInput
    {
        [JsonProperty("type")]
        public string Type { get; } = "html";

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("encoding")]
        public string Encoding { get; set; } = "iso-8859-1";

        [JsonProperty("options")]
        public IHtmlSubstitutionOptions Options { get; set; } = new HtmlSubstitutionOptions();

        [JsonProperty("columns")]
        public IHtmlSubstitutionColumns ColumnSettings { get; set; } = new HtmlSubstitutionColumns();

        [JsonProperty("absences")]
        public IHtmlAbsenceSettings AbsenceSettings { get; } = new HtmlAbsenceSettings();

        [JsonProperty("type_replacements")]
        public Dictionary<string, string> TypeReplacements { get; set; } = new Dictionary<string, string>();

        [JsonProperty("remove_types")]
        public string[] RemoveSubstitutionsWithTypes { get; set; } = Array.Empty<string>();
    }
}
