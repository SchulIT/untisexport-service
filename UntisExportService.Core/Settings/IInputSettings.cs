using UntisExportService.Core.Settings.Inputs.Exams;
using UntisExportService.Core.Settings.Inputs.Rooms;
using UntisExportService.Core.Settings.Inputs.Substitutions;
using UntisExportService.Core.Settings.Inputs.Supervisions;
using UntisExportService.Core.Settings.Inputs.Timetable;
using UntisExportService.Core.Settings.Inputs.Tuitions;

namespace UntisExportService.Core.Settings
{
    public interface IInputSettings
    {
        ISubstitutionInput Substitutions { get; }

        IExamInput Exams { get; }

        ISupervisionInput Supervisions { get; }

        ITuitionInput Tuitions { get; }

        IRoomInput Rooms { get; }

        ITimetableInput Timetable { get; }
    }
}
