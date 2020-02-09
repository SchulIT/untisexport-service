namespace UntisExportService.Core.StudyGroups
{
    public interface IStudyGroupResolver
    {
        void Initialize();

        string Resolve(string grade, string subject);
    }
}
