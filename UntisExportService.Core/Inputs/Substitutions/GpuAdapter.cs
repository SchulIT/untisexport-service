using SchulIT.UntisExport.Substitutions;
using SchulIT.UntisExport.Substitutions.Gpu;
using System.Threading.Tasks;
using UntisExportService.Core.Settings.Inputs.Substitutions;

namespace UntisExportService.Core.Inputs.Substitutions
{
    public class GpuAdapter : IAdapter
    {
        public string SearchPattern { get { return "GPU017.txt"; } }

        private ISubstitutionExporter exporter;

        public GpuAdapter(ISubstitutionExporter exporter)
        {
            this.exporter = exporter;
        }

        public Task<SubstitutionExportResult> GetSubstitutionsAsync(string contents, ISubstitutionInput settings)
        {
            var gpuInput = settings as IGpuSubstitutionInput;
            return exporter.ParseGpuAsync(contents, new SubstitutionExportSettings { Delimiter = gpuInput.Delimiter });
        }

        public bool Use(ISubstitutionInput settings)
        {
            return settings is IGpuSubstitutionInput;
        }
    }
}
