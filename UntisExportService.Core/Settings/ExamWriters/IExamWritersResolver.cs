using JsonSubTypes;
using Newtonsoft.Json;
using UntisExportService.Core.Settings.ExamWriters.Schild.Json;

namespace UntisExportService.Core.Settings.ExamWriters
{
    [JsonConverter(typeof(JsonSubtypes), nameof(Type))]
    [JsonSubtypes.KnownSubType(typeof(SchildExamWritersResolver), "schild")]
    public interface IExamWritersResolver
    {
        string Type { get; }
    }
}
