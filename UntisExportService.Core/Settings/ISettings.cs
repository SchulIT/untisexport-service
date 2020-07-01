using System.Collections.Generic;
using UntisExportService.Core.Settings.ExamWriters;
using UntisExportService.Core.Settings.External;
using UntisExportService.Core.Settings.Outputs;
using UntisExportService.Core.Settings.Tuitions;

namespace UntisExportService.Core.Settings
{
    public interface ISettings
    {
        int SyncThresholdInSeconds { get; }

        IInputSettings Inputs { get; }

        ITuitionResolver Tuition { get; }

        IList<IOutput> Outputs { get; }

        IExamWritersResolver ExamWriters { get; }

        IList<IExternal> ExternalServices { get; }
    }
}