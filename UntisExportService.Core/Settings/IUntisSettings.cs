using SchulIT.UntisExport;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings
{
    public interface IUntisSettings
    {
        bool FixBrokenPTags { get; }

        string DateTimeFormat { get; }

        bool InlcudeAbsentValues { get; }

        string[] EmptyValues { get; }

        IUntisColumnSettings ColumnSettings { get; } 

        Dictionary<string, string> TypeReplacements { get; }

        string[] RemoveSubstitutionsWithTypes { get; }

    }
}
