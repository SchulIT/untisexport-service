using SchulIT.UntisExport.Model;
using System.Collections.Generic;
using UntisExportService.Core.Model;
using UntisExportService.Core.Settings;

namespace UntisExportService.Core.Upload
{
    public interface IModelStrategy
    {
        bool IsSupported(ISettings settings);

        IEnumerable<ISubstitution> GetSubstitutions(IEnumerable<Substitution> substitutions);

        IEnumerable<IInfotext> GetInfotexts(IEnumerable<Infotext> infotexts);

        IEnumerable<IAbsence> GetAbsences(IEnumerable<Absence> absences);
    }
}
