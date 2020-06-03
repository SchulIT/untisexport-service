using UntisExportService.Core.Settings.Tuitions;

namespace UntisExportService.Core.Tuitions
{
    public abstract class AbstractTuitionResolveStrategy<T> : ITuitionResolveStrategy<T>
        where T : class, Settings.Tuitions.ITuitionResolver
    {
        public abstract void Initialize(T inputSetting);

        public abstract string ResolveStudyGroup(string grade);

        public abstract string ResolveStudyGroup(string grade, string subject, string teacher);

        public abstract string ResolveTuition(string grade, string subject, string teacher);

        public void Initialize(Settings.Tuitions.ITuitionResolver inputSetting)
        {
            Initialize(inputSetting as T);
        }

        public bool Supports(Settings.Tuitions.ITuitionResolver inputSetting)
        {
            return inputSetting is T;
        }

        
    }
}
