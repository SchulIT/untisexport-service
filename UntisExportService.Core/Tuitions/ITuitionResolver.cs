namespace UntisExportService.Core.Tuitions
{
    public interface ITuitionResolver
    {
        void Initialize();

        string ResolveTuition(string grade, string subject, string teacher);

        string ResolveStudyGroup(string grade, string subject, string teacher);

        string ResolveStudyGroup(string grade);
    }
}
