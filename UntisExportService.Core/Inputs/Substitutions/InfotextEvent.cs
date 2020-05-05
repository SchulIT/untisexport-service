using Redbus.Events;
using SchulIT.UntisExport.Substitutions;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Substitutions
{
    public class InfotextEvent : EventBase
    {
        public IEnumerable<Infotext> Infotexts { get; private set; }

        public InfotextEvent(IEnumerable<Infotext> infotexts)
        {
            Infotexts = infotexts;
        }
    }
}
