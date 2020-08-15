using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using UntisExportService.Core.Inputs.Exams;
using UntisExportService.Core.Inputs.Rooms;
using UntisExportService.Core.Inputs.Substitutions;
using UntisExportService.Core.Inputs.Supervisions;
using UntisExportService.Core.Inputs.Timetable;
using UntisExportService.Core.Inputs.Tuitions;
using UntisExportService.Core.Settings.Outputs;

namespace UntisExportService.Core.Outputs.Json
{
    public class JsonOutputHandler : OutputHandlerBase<IFileOutput>
    {
        public override bool CanHandleAbsences { get { return true; } }

        public override bool CanHandleExams { get { return true; } }

        public override bool CanHandleInfotexts { get { return true; } }

        public override bool CanHandleRooms { get { return true; } }

        public override bool CanHandleSubstitutions { get { return true; } }

        public override bool CanHandleSupervisions { get { return true; } }

        public override bool CanHandleTimetable { get { return true; } }

        public override bool CanHandleTuitions { get { return true; } }

        public override bool CanHandleFreeLessons { get { return true; } }

        private readonly ILogger<JsonOutputHandler> logger;

        public JsonOutputHandler(ILogger<JsonOutputHandler> logger)
        {
            this.logger = logger;
        }

        private async Task WriteJson(object data, IFileOutput outputSettings, string filename)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var path = Path.Combine(outputSettings.Path, filename);

            logger.LogInformation($"Writing output to {path}...");

            using (var writer = new StreamWriter(path))
            {
                await writer.WriteAsync(json);
            }

            logger.LogInformation("Done.");
        }

        protected override Task HandleAbsenceEvent(AbsenceEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.Absences, outputSettings, "absences.json");
        }

        protected override Task HandleExamEvent(ExamEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.Exams, outputSettings, "exams.json");
        }

        protected override Task HandleInfotextEvent(InfotextEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.Infotexts, outputSettings, "infotexts.json");
        }

        protected override Task HandleRoomEvent(RoomEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.Rooms, outputSettings, "rooms.json");
        }

        protected override Task HandleSubstitutionEvent(SubstitutionEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.Substitutions, outputSettings, "substitutions.json");
        }

        protected override Task HandleSupervisionEvent(SupervisionEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.Supervisions, outputSettings, "supervisions.json");
        }

        protected override Task HandleTimetableEvent(TimetableEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.Lessons, outputSettings, "lessons.json");
        }

        protected override Task HandleTuitionEvent(TuitionEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.Tuitions, outputSettings, "tuitions.json");
        }

        protected override Task HandleFreeLessonEvent(FreeLessonEvent @event, IFileOutput outputSettings)
        {
            return WriteJson(@event.FreeLessons, outputSettings, "free_lessons.json");
        }
    }
}
