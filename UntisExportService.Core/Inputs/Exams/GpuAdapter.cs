using SchulIT.UntisExport.Exams;
using SchulIT.UntisExport.Exams.Gpu;
using System;
using System.Threading.Tasks;
using UntisExportService.Core.Settings.Inputs.Exams;

namespace UntisExportService.Core.Inputs.Exams
{
    public class GpuAdapter : IAdapter
    {
        public string SearchPattern { get { return "GPU017.txt"; } }

        private readonly IExamExporter examExporter;

        public GpuAdapter(IExamExporter examExporter)
        {
            this.examExporter = examExporter;
        }

        public Task<ExamExportResult> GetExamsAsync(string contents, IExamInput settings)
        {
            throw new NotImplementedException();
        }

        public bool Use(IExamInput settings)
        {
            return settings is IGpuExamInput;
        }
    }
}
