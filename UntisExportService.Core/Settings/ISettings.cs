namespace UntisExportService.Core.Settings
{
    public interface ISettings
    {
        bool IsDebugModeEnabled { get; }

        IEndpointSettings Endpoint { get; }

        IUntisSettings Untis { get; }

        string HtmlPath { get; }

        bool IsServiceEnabled { get; }
    }
}