namespace UntisExportService.Core.Settings.Outputs
{
    public interface IEndpointSettings
    {
        string Url { get; }

        string Token { get; }

        string ResponsePath { get; }
    }
}