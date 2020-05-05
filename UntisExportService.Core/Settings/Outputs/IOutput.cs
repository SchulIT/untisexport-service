using JsonSubTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using UntisExportService.Core.Settings.Outputs.Json;

namespace UntisExportService.Core.Settings.Outputs
{
    [JsonConverter(typeof(JsonSubtypes), nameof(Type))]
    [JsonSubtypes.KnownSubType(typeof(IccOutput), "icc")]
    [JsonSubtypes.KnownSubType(typeof(FileOutput), "json")]
    public interface IOutput
    {
        IList<string> Entities { get; }

        string Type { get; }
    }
}
