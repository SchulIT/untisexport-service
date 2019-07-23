using SchulIT.UntisExport;
using System.Collections.Generic;

namespace UntisExportService.Core.Settings
{
    public interface IUntisSettings
    {

        int SyncThresholdInSeconds { get; }

        bool RemoveExams { get; }

        bool FixBrokenPTags { get; }

        string DateTimeFormat { get; }

        List<string> EmptyValues { get; }

        bool InlcudeAbsentValues { get; }

        IUntisColumnSettings ColumnSettings { get; } 
    }
}
