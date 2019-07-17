namespace UntisExportService.Core.FileSystem
{
    public interface IFileSystemWatcher
    {
        string Path { get; set; }

        event OnChangedEventHandler Changed;
    }
}
