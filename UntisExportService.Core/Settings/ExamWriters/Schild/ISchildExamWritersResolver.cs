using System.Collections.Generic;

namespace UntisExportService.Core.Settings.ExamWriters.Schild
{
    public interface ISchildExamWritersResolver : IExamWritersResolver
    {
        List<SchildSection> Sections { get; }

        List<SchildExamWriterRule> Rules { get; }
    }
}
