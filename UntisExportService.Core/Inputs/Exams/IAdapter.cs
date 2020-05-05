using SchulIT.UntisExport.Exams;
using System.Threading.Tasks;
using UntisExportService.Core.Settings.Inputs.Exams;

namespace UntisExportService.Core.Inputs.Exams
{
    public interface IAdapter
    {
        string SearchPattern { get; }

        Task<ExamExportResult> GetExamsAsync(string contents, IExamInput settings);

        bool Use(IExamInput settings);
    }
}
