namespace UntisExportService.Core.Settings
{
    public interface ISettings
    {
        bool IsDebugModeEnabled { get; }

        bool IsServiceEnabled { get; }

        string HtmlPath { get; }

        int SyncThresholdInSeconds { get; }

        string Encoding { get; }

        IEndpointSettings Endpoint { get; }

        IUntisSettings Untis { get; }

        string StudyGroupsJsonFile { get; }
    }
}