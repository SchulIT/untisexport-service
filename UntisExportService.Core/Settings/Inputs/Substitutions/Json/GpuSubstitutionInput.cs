using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Inputs.Substitutions.Json
{
    public class GpuSubstitutionInput : IGpuSubstitutionInput
    {
        [JsonProperty("type")]
        public string Type { get; } = "gpu";

        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("delimiter")]
        public string Delimiter { get; set; } = ",";

        [JsonProperty("encoding")]
        public string Encoding { get; set; } = "iso-8859-1";

        [JsonProperty("type_replacements")]
        public Dictionary<string, string> TypeReplacements { get; set; } = new Dictionary<string, string>();

        [JsonProperty("remove_types")]
        public string[] RemoveSubstitutionsWithTypes { get; set; } = Array.Empty<string>();
    }
}
