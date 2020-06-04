using JsonSubTypes;
using Newtonsoft.Json;
using UntisExportService.Core.Settings.External.Schild.Json;

namespace UntisExportService.Core.Settings.External
{
    [JsonConverter(typeof(JsonSubtypes), nameof(Type))]
    [JsonSubtypes.KnownSubType(typeof(SchildSettings), "schild")]
    public interface IExternal
    {
        string Type { get; }
    }
}
