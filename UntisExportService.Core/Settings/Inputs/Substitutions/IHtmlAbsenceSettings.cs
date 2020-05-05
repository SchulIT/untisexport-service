namespace UntisExportService.Core.Settings.Inputs.Substitutions
{
    public interface IHtmlAbsenceSettings
    {
        bool ParseAbsences { get; }

        string TeacherIdentifier { get; }

        string StudyGroupIdentifier { get; }
    }
}
