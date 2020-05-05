using Newtonsoft.Json;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Outputs.Json
{
    public class WeekMapping : IWeekMapping
    {
        [JsonProperty("weeks")]
        public Dictionary<int, string> Weeks { get; set; }

        [JsonProperty("use_week_modulo")]
        public bool UseWeekModulo { get; set; }
    }
}
