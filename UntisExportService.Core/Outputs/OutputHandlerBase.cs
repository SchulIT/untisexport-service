using System;
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
    public abstract class OutputHandlerBase<T> : IOutputHandler
        where T : class, IOutput
    {
        public abstract bool CanHandleAbsences { get; }
        public abstract bool CanHandleExams { get; }
        public abstract bool CanHandleInfotexts { get; }
        public abstract bool CanHandleRooms { get; }
        public abstract bool CanHandleSubstitutions { get; }
        public abstract bool CanHandleSupervisions { get; }
        public abstract bool CanHandleTimetable { get; }
        public abstract bool CanHandleTuitions { get; }

        private T CastSettings(IOutput outputSettings)
        {
            if (!(outputSettings is T settings))
            {
                throw new ArgumentException($"outputSettings is not of type {typeof(T).GetType()}.");
            }

            return settings;
        }

        public Task HandleAbsenceEvent(AbsenceEvent @event, IOutput outputSettings)
        {
            return HandleAbsenceEvent(@event, CastSettings(outputSettings));
        }

        protected abstract Task HandleAbsenceEvent(AbsenceEvent @event, T outputSettings);

        public Task HandleExamEvent(ExamEvent @event, IOutput outputSettings)
        {
            return HandleExamEvent(@event, CastSettings(outputSettings));
        }

        protected abstract Task HandleExamEvent(ExamEvent @event, T outputSettings);

        public Task HandleInfotextEvent(InfotextEvent @event, IOutput outputSettings)
        {
            return HandleInfotextEvent(@event, CastSettings(outputSettings));
        }

        protected abstract Task HandleInfotextEvent(InfotextEvent @event, T outputSettings);

        public Task HandleRoomEvent(RoomEvent @event, IOutput outputSettings)
        {
            return HandleRoomEvent(@event, CastSettings(outputSettings));
        }

        protected abstract Task HandleRoomEvent(RoomEvent @event, T outputSettings);

        public Task HandleSubstitutionEvent(SubstitutionEvent @event, IOutput outputSettings)
        {
            return HandleSubstitutionEvent(@event, CastSettings(outputSettings));
        }

        protected abstract Task HandleSubstitutionEvent(SubstitutionEvent @event, T outputSettings);

        public Task HandleSupervisionEvent(SupervisionEvent @event, IOutput outputSettings)
        {
            return HandleSupervisionEvent(@event, CastSettings(outputSettings));
        }

        protected abstract Task HandleSupervisionEvent(SupervisionEvent @event, T outputSettings);

        public Task HandleTimetableEvent(TimetableEvent @event, IOutput outputSettings)
        {
            return HandleTimetableEvent(@event, CastSettings(outputSettings));
        }

        protected abstract Task HandleTimetableEvent(TimetableEvent @event, T outputSettings);

        public Task HandleTuitionEvent(TuitionEvent @event, IOutput outputSettings)
        {
            return HandleTuitionEvent(@event, CastSettings(outputSettings));
        }

        protected abstract Task HandleTuitionEvent(TuitionEvent @event, T outputSettings);
    }
}
