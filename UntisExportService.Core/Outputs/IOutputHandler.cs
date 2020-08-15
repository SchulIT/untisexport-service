using System.Threading.Tasks;
using UntisExportService.Core.Inputs.Exams;
using UntisExportService.Core.Inputs.Rooms;
using UntisExportService.Core.Inputs.Substitutions;
using UntisExportService.Core.Inputs.Supervisions;
using UntisExportService.Core.Inputs.Timetable;
using UntisExportService.Core.Inputs.Tuitions;
using UntisExportService.Core.Settings.Outputs;

namespace UntisExportService.Core.Outputs
{
    public interface IOutputHandler
    {
        bool CanHandleAbsences { get; }

        bool CanHandleExams { get; }

        bool CanHandleInfotexts { get; }

        bool CanHandleRooms { get; }

        bool CanHandleSubstitutions { get; }

        bool CanHandleSupervisions { get; }

        bool CanHandleTimetable { get; }

        bool CanHandleTuitions { get; }

        bool CanHandleFreeLessons { get; }

        Task HandleSubstitutionEvent(SubstitutionEvent @event, IOutput outputSettings);

        Task HandleInfotextEvent(InfotextEvent @event, IOutput outputSettings);

        Task HandleAbsenceEvent(AbsenceEvent @event, IOutput outputSettings);

        Task HandleTimetableEvent(TimetableEvent @event, IOutput outputSettings);

        Task HandleExamEvent(ExamEvent @event, IOutput outputSettings);

        Task HandleRoomEvent(RoomEvent @event, IOutput outputSettings);

        Task HandleSupervisionEvent(SupervisionEvent @event, IOutput outputSettings);

        Task HandleTuitionEvent(TuitionEvent @event, IOutput outputSettings);

        Task HandleFreeLessonEvent(FreeLessonEvent @event, IOutput outputSettings);
    }
}
