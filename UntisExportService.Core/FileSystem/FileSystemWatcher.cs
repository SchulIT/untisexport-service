using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace UntisExportService.Core.FileSystem
{
    public class FileSystemWatcher : IFileSystemWatcher
    {
        private string path;

        /// <summary>
        /// Patch to the file or directory that needs to be watched.
        /// </summary>
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                ConfigureWatcher();
            }
        }

        /// <summary>
        /// Event which gets raised in case the directory or its content changes.
        /// </summary>
        public event OnChangedEventHandler Changed;

        protected void OnChanged(OnChangedEventArgs args)
        {
            Changed?.Invoke(this, args);
        }

        private readonly System.IO.FileSystemWatcher watcher;
        private readonly ILogger<FileSystemWatcher> logger;

        public FileSystemWatcher(ILogger<FileSystemWatcher> logger)
        {
            this.logger = logger;

            watcher = new System.IO.FileSystemWatcher
            {
                IncludeSubdirectories = true
            };

            watcher.Created += OnFileSystemWatcherChanged;
            watcher.Changed += OnFileSystemWatcherChanged;
            watcher.Renamed += OnFileSystemWatcherRenamed;
            watcher.EnableRaisingEvents = false;
        }

        private void OnFileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            OnChanged(new OnChangedEventArgs());
        }

        private void OnFileSystemWatcherRenamed(object sender, RenamedEventArgs e)
        {
            OnChanged(new OnChangedEventArgs());
        }

        private void ConfigureWatcher()
        {
            try
            {
                watcher.Path = Path;
                watcher.EnableRaisingEvents = true;

                logger.LogDebug($"Enabled watching {Path}.");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to watch {Path}.");
            }
        }
    }
}
