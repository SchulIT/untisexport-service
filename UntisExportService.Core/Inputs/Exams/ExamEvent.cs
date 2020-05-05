using Redbus.Events;
using SchulIT.UntisExport.Exams;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Exams
{
    public class ExamEvent : EventBase
    {
        public IEnumerable<Exam> Exams { get; private set; }

        public ExamEvent(IEnumerable<Exam> exams)
        {
            Exams = exams;
        }
    }
}
