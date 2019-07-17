using Microsoft.Extensions.Logging;

namespace UntisExportService.Core.FileSystem
{
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        private ILogger<FileSystemWatcher> logger;

        public FileSystemWatcherFactory(ILogger<FileSystemWatcher> logger)
        {
            this.logger = logger;
        }

        public IFileSystemWatcher CreateWatcher()
        {
            return new FileSystemWatcher(logger);
        }
    }
}
