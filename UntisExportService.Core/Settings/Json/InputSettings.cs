using Newtonsoft.Json;
using UntisExportService.Core.Json;
using UntisExportService.Core.Settings.Inputs.Exams;
using UntisExportService.Core.Settings.Inputs.Rooms;
using UntisExportService.Core.Settings.Inputs.Rooms.Json;
using UntisExportService.Core.Settings.Inputs.Substitutions;
using UntisExportService.Core.Settings.Inputs.Supervisions;
using UntisExportService.Core.Settings.Inputs.Supervisions.Json;
using UntisExportService.Core.Settings.Inputs.Timetable;
using UntisExportService.Core.Settings.Inputs.Timetable.Json;
using UntisExportService.Core.Settings.Inputs.Tuitions;
using UntisExportService.Core.Settings.Inputs.Tuitions.Json;

namespace UntisExportService.Core.Settings.Json
{
    public class InputSettings : IInputSettings
    {
        [JsonProperty("substitutions")]
        public ISubstitutionInput Substitutions { get; set; }

        [JsonProperty("exams")]
        public IExamInput Exams { get; set; }

        [JsonProperty("supervisions")]
        [JsonConverter(typeof(ConcreteTypeConverter<SupervisionInput>))]
        public ISupervisionInput Supervisions { get; set; }

        [JsonProperty("tuitions")]
        [JsonConverter(typeof(ConcreteTypeConverter<TuitionInput>))]
        public ITuitionInput Tuitions { get; set; }

        [JsonProperty("rooms")]
        [JsonConverter(typeof(ConcreteTypeConverter<RoomInput>))]
        public IRoomInput Rooms { get; set; }

        [JsonProperty("timetable")]
        [JsonConverter(typeof(ConcreteTypeConverter<HtmlTimetableInput>))]
        public ITimetableInput Timetable { get; set; }
    }
}
