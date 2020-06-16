using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Tuitions.Json
{
    public class SchildTuitionResolver : ISchildTuitionResolver
    {
        [JsonProperty("type")]
        public string Type { get; } = "schild";

       
    }
}
