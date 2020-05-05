using Redbus.Events;
using SchulIT.UntisExport.Timetable;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Timetable
{
    public class TimetableEvent : EventBase
    {
        public string Period;

        public List<Lesson> Lessons { get; private set; }

        public TimetableEvent(string period, List<Lesson> lessons)
        {
            Period = period;
            Lessons = lessons;
        }
    }
}
