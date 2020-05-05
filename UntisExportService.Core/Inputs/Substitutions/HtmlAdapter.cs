using SchulIT.UntisExport.Substitutions;
using SchulIT.UntisExport.Substitutions.Html;
using System.Threading.Tasks;
using UntisExportService.Core.Extensions;
using UntisExportService.Core.Settings.Inputs.Substitutions;

namespace UntisExportService.Core.Inputs.Substitutions
{
    public class HtmlAdapter : IAdapter
    {
        public string SearchPattern { get { return "*.htm"; } }

        private ISubstitutionExporter exporter;

        public HtmlAdapter(ISubstitutionExporter exporter)
        {
            this.exporter = exporter;
        }

        public Task<SubstitutionExportResult> GetSubstitutionsAsync(string contents, ISubstitutionInput settings)
        {
            var htmlSettings = settings as IHtmlSubstitutionInput;
            return exporter.ParseHtmlAsync(contents, htmlSettings.ToUntis());
        }

        public bool Use(ISubstitutionInput settings)
        {
            return settings is IHtmlSubstitutionInput;
        }
    }
}
