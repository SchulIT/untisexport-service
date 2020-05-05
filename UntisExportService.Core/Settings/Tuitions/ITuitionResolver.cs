using JsonSubTypes;
using Newtonsoft.Json;
using UntisExportService.Core.Settings.Tuitions.Json;

namespace UntisExportService.Core.Settings.Tuitions
{
    [JsonConverter(typeof(JsonSubtypes), nameof(Type))]
    [JsonSubtypes.KnownSubType(typeof(SchildTuitionResolver), "schild")]
    public interface ITuitionResolver
    {
        string Type { get; }
    }
}
