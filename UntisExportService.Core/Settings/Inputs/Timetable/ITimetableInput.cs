using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Inputs.Timetable
{
    public interface ITimetableInput : IHtmlInput
    {
        int FirstLesson { get; }

        bool UseWeeks { get; }

        bool OnlyLastPeriod { get; }

        List<string> Subjects { get; }

        List<string> Grades { get; }
    }
}
