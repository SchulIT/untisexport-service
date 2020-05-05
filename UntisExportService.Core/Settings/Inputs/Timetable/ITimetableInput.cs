using System;
using System.Collections.Generic;
using System.Text;

namespace UntisExportService.Core.Settings.Inputs.Timetable
{
    public interface ITimetableInput : IHtmlInput
    {
        int FirstLesson { get; }

        bool UseWeeks { get; }
    }
}
