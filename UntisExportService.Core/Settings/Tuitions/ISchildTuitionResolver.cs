using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Tuitions
{
    public interface ISchildTuitionResolver : ITuitionResolver
    {
        List<string> GradesWithCourseNameAsSubject { get; }

        List<UntisToExternalRule> SubjectConversationRules { get; }
    }
}
