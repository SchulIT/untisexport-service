using SchulIT.SchildExport.Models;
using System;

namespace UntisExportService.Core.External.Schild
{
    public class TuitionStudyGroupTuple : Tuple<Tuition, StudyGroup, string>
    {
        public Tuition Tuition { get { return Item1; } }

        public StudyGroup StudyGroup { get { return Item2; } }

        public string Subject { get { return Item3; } }

        public TuitionStudyGroupTuple(Tuition tuition, StudyGroup studyGroup, string subject)
            : base(tuition, studyGroup, subject) { }
    }
}
