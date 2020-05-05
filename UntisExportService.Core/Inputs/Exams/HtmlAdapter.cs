using SchulIT.UntisExport.Exams.Html;
using System.Text;
using System.Threading.Tasks;
using UntisExportService.Core.Extensions;
using UntisExportService.Core.FileSystem;
using UntisExportService.Core.Settings.Inputs.Exams;

namespace UntisExportService.Core.Inputs.Exams
{
    public class HtmlAdapter : IAdapter
    {
        public string SearchPattern => "*.htm";

        private readonly IExamExporter exporter;
        private readonly IFileReader fileReader;

        public HtmlAdapter(IExamExporter exporter, IFileReader fileReader)
        {
            this.exporter = exporter;
            this.fileReader = fileReader;
        }

        public async Task<SchulIT.UntisExport.Exams.ExamExportResult> GetExamsAsync(string html, IExamInput settings)
        {
            var exportSettings = settings as IHtmlExamInput;

            return await exporter.ParseHtmlAsync(html, new ExamExportSettings
            {
                DateTimeFormat = exportSettings.DateTimeFormat,
                ColumnSettings = exportSettings.Columns.ToUntis()
            });
        }

        public bool Use(IExamInput settings)
        {
            return settings is IHtmlExamInput;
        }
    }
}
