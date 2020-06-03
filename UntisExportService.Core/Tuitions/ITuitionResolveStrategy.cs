using UntisExportService.Core.Settings.Tuitions;

namespace UntisExportService.Core.Tuitions
{
    public interface ITuitionResolveStrategy
    {
        bool Supports(Settings.Tuitions.ITuitionResolver inputSetting);

        void Initialize(Settings.Tuitions.ITuitionResolver inputSetting);

        string ResolveStudyGroup(string grade);

        string ResolveStudyGroup(string grade, string subject, string teacher);

        string ResolveTuition(string grade, string subject, string teacher);
    }

    public interface ITuitionResolveStrategy<T> : ITuitionResolveStrategy
        where T : Settings.Tuitions.ITuitionResolver
    {
        void Initialize(T inputSetting);
    }
}
