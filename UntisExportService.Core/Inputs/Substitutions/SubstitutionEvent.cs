using Redbus.Events;
using SchulIT.UntisExport.Substitutions;
using System.Collections.Generic;

namespace UntisExportService.Core.Inputs.Substitutions
{
    public class SubstitutionEvent : EventBase
    {
        public IEnumerable<Substitution> Substitutions { get; private set; }

        public SubstitutionEvent(IEnumerable<Substitution> substitutions)
        {
            Substitutions = substitutions;
        }
    }
}
