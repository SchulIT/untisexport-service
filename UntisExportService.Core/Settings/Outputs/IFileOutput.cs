namespace UntisExportService.Core.Settings.Outputs
{
    public interface IFileOutput : IOutput
    {
        string Path { get; }
    }
}
