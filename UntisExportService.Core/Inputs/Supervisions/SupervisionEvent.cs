using Redbus.Events;
using SchulIT.UntisExport.Supervisions;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Supervisions
{
    public class SupervisionEvent : EventBase
    {
        public IEnumerable<Supervision> Supervisions { get; private set; }

        public SupervisionEvent(IEnumerable<Supervision> supervisions)
        {
            Supervisions = supervisions;
        }
    }
}
