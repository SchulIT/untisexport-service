using JsonSubTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using UntisExportService.Core.Settings.Inputs.Substitutions.Json;

namespace UntisExportService.Core.Settings.Inputs.Substitutions
{
    [JsonConverter(typeof(JsonSubtypes), nameof(Type))]
    [JsonSubtypes.KnownSubType(typeof(GpuSubstitutionInput), "gpu")]
    [JsonSubtypes.KnownSubType(typeof(HtmlSubstitutionInput), "html")]
    public interface ISubstitutionInput
    {
        string Type { get; }

        string Path { get; }

        string Encoding { get; }

        Dictionary<string, string> TypeReplacements { get; }

        string[] RemoveSubstitutionsWithTypes { get; }
    }
}
