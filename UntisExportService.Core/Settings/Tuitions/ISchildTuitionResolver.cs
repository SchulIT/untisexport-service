using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Tuitions
{
    public interface ISchildTuitionResolver : ITuitionResolver
    {
        string ConnectionString { get; }

        List<string> GradesWithCourseNameAsSubject { get; }

        Dictionary<string, string> SchildToUntisSubjectMap { get; }
    }
}
