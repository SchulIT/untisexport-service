namespace UntisExportService.Core.Settings
{
    public interface ISettings
    {
        bool IsDebugModeEnabled { get; }

        IEndpointSettings Endpoint { get; }

        IUntisSettings Untis { get; }

        string HtmlPath { get; }

        string Encoding { get; }

        bool IsServiceEnabled { get; }
    }
}