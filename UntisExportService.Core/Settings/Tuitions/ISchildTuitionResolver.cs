using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Tuitions
{
    public interface ISchildTuitionResolver : ITuitionResolver
    {
        

        List<string> GradesWithCourseNameAsSubject { get; }

        Dictionary<string, string> SchildToUntisSubjectMap { get; }
    }
}
