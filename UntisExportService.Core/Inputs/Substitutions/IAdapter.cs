using SchulIT.UntisExport.Substitutions;
using System.Threading.Tasks;
using UntisExportService.Core.Settings.Inputs.Substitutions;

namespace UntisExportService.Core.Inputs.Substitutions
{
    public interface IAdapter
    {
        string SearchPattern { get; }

        Task<SubstitutionExportResult> GetSubstitutionsAsync(string contents, ISubstitutionInput settings);

        bool Use(ISubstitutionInput settings);
    }
}
