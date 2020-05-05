using System.Collections.Generic;
using UntisExportService.Core.Settings.Outputs;
using UntisExportService.Core.Settings.Tuitions;

namespace UntisExportService.Core.Settings
{
    public interface ISettings
    {
        bool IsDebugModeEnabled { get; }

        int SyncThresholdInSeconds { get; }

        IInputSettings Inputs { get; }

        ITuitionResolver Tuition { get; }

        IList<IOutput> Outputs { get; }
    }
}