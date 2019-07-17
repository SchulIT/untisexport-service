using Newtonsoft.Json;
using System;

namespace UntisExportService.Core.Model
{
    public class IccInfotext
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
