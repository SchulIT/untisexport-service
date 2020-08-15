using Redbus.Events;
using SchulIT.UntisExport.Substitutions;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Substitutions
{
    public class FreeLessonEvent : EventBase
    {
        public IEnumerable<FreeLessonsTimespan> FreeLessons { get; private set; }

        public FreeLessonEvent(IEnumerable<FreeLessonsTimespan> freeLessons)
        {
            FreeLessons = freeLessons;
        }
    }
}
