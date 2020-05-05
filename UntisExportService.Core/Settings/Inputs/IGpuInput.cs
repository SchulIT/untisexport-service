namespace UntisExportService.Core.Settings.Inputs
{
    public interface IGpuInput
    {
        string Path { get; }

        string Delimiter { get; }

        string Encoding { get; }
    }
}
