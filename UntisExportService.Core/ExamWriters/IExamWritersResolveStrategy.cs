using SchulIT.UntisExport.Exams;
using System;
using System.Collections.Generic;

namespace UntisExportService.Core.ExamWriters
{
    public interface IExamWritersResolveStrategy
    {
        bool Supports(Settings.ExamWriters.IExamWritersResolver inputSetting);

        void Initialize(Settings.ExamWriters.IExamWritersResolver inputSetting);

        List<string> Resolve(string tuition, Exam exam, string start, string end);
    }
}
