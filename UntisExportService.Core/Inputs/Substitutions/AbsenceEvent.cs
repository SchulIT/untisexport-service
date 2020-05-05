using Redbus.Events;
using SchulIT.UntisExport.Substitutions;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Substitutions
{
    public class AbsenceEvent : EventBase
    {
        public IEnumerable<Absence> Absences { get; private set; }

        public AbsenceEvent(IEnumerable<Absence> absences)
        {
            Absences = absences;
        }
    }
}
