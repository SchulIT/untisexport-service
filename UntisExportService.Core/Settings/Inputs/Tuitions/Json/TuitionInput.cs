﻿using Newtonsoft.Json;

namespace UntisExportService.Core.Settings.Inputs.Tuitions.Json
{
    public class TuitionInput : ITuitionInput
    {
        [JsonProperty("path")]
        public string Path { get; set; } = string.Empty;

        [JsonProperty("delimiter")]
        public string Delimiter { get; set; } = ",";

        [JsonProperty("encoding")]
        public string Encoding { get; set; } = "iso-8859-1";
    }
}
