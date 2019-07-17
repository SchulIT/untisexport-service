namespace UntisExportService.Core.FileSystem
{
    public interface IFileSystemWatcherFactory
    {
        IFileSystemWatcher CreateWatcher();
    }
}
