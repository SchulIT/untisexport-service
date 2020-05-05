using Redbus.Events;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Tuitions
{
    public class TuitionEvent : EventBase
    {
        public IEnumerable<SchulIT.UntisExport.Tuitions.Tuition> Tuitions { get; private set; }

        public TuitionEvent(IEnumerable<SchulIT.UntisExport.Tuitions.Tuition> tuitions)
        {
            Tuitions = tuitions;
        }
    }
}
