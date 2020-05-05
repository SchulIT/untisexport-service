using JsonSubTypes;
using Newtonsoft.Json;
using UntisExportService.Core.Settings.Inputs.Exams.Json;

namespace UntisExportService.Core.Settings.Inputs.Exams
{
    [JsonConverter(typeof(JsonSubtypes), nameof(Type))]
    [JsonSubtypes.KnownSubType(typeof(GpuExamInput), "gpu")]
    [JsonSubtypes.KnownSubType(typeof(HtmlExamInput), "html")]
    public interface IExamInput
    {
        string Type { get; }

        string Path { get; }

        string Encoding { get; }
    }
}
