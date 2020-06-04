namespace UntisExportService.Core.Settings.External.Schild
{
    public interface ISchildSettings : IExternal
    {
        string ConnectionString { get; }
    }
}
