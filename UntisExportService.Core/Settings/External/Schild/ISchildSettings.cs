using System.Collections.Generic;

namespace UntisExportService.Core.Settings.External.Schild
{
    public interface ISchildSettings : IExternal
    {
        string ConnectionString { get; }

        List<string> GradesWithCourseNameAsSubject { get; }

        List<string> SubjectsWithCourseNameAsSubject { get; }

        List<UntisToSchildRule> SubjectConversationRules { get; }
    }
}
