using SchulIT.UntisExport.Exams;
using System.Collections.Generic;

namespace UntisExportService.Core.ExamWriters
{
    public interface IExamWritersResolver
    {
        void Initialize();

        List<string> ResolveStudents(string tuition, Exam exam);
    }
}
