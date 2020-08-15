namespace UntisExportService.Core.Settings.Inputs.Substitutions
{
    public interface IHtmlFreeLessonSettings
    {
        bool ParseFreeLessons { get; } 

        bool RemoveInfotext { get; } 

        string FreeLessonIdentifier { get; } 

        string LessonIdentifier { get; }
    }
}
