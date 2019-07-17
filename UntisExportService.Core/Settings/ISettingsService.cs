namespace UntisExportService.Core.Settings
{
    public interface ISettingsService
    {
        ISettings Settings { get; }

        event SettingsChangedEventHandler Changed;
    }
}
