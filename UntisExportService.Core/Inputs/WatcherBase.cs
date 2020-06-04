using Microsoft.Extensions.Logging;
using Redbus.Events;
using Redbus.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UntisExportService.Core.FileSystem;

namespace UntisExportService.Core.Inputs
{
    /// <summary>
    /// Abstract class which all watchers should inherit from as it 
    /// provides the basic functionality of watching a certain directory.
    /// Also, this watcher implementation ensures that the further logic
    /// of handling the input is deferred by a certain amount of time
    /// because the watcher gets triggered when the first file is created, but
    /// Untis creates multiple files.
    /// </summary>
    public abstract class WatcherBase<T> : IWatcher<T>
    {
        public int SyncThresholdInSeconds { get; set; }

        protected abstract bool CanStart { get; }

        private bool isExportRunning = false;
        private readonly object exportLock = new object();

        private DateTime? lastTrigger = null;

        protected readonly IFileSystemWatcher watcher;
        protected readonly IEventBus eventBus;
        private readonly ILogger logger;

        protected WatcherBase(IFileSystemWatcher watcher, IEventBus eventBus, ILogger logger)
        {
            this.watcher = watcher;
            this.eventBus = eventBus;
            this.logger = logger;
        }

        public abstract void Configure(T settings);

        protected abstract Task<IEnumerable<EventBase>> OnFilesChanged();

        protected abstract string GetPath();

        public void Start()
        {
            if (CanStart)
            {
                watcher.Path = GetPath();
                watcher.Changed += OnFilesChanged;
            }
            else
            {
                logger.LogInformation("Do not watch as settings are null.");
            }
        }

        public void Stop()
        {
            watcher.Changed -= OnFilesChanged;
        }

        protected IEnumerable<EventBase> FromSingleEvent(EventBase @event)
        {
            return new EventBase[] { @event };
        }

        private async void OnFilesChanged(IFileSystemWatcher sender, OnChangedEventArgs args)
        {
            try
            {
                logger.LogInformation("Detected filesystem changed.");

                lock (exportLock)
                {
                    lastTrigger = DateTime.Now;

                    if (isExportRunning)
                    {
                        logger.LogDebug("Export already running, skipping.");
                        return;
                    }

                    isExportRunning = true;
                }

                if (SyncThresholdInSeconds > 0)
                {
                    var elapsed = DateTime.Now - lastTrigger.Value;
                    if(elapsed.TotalSeconds < SyncThresholdInSeconds)
                    {
                        logger.LogDebug($"Waiting for Untis to create all files.");
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }

                var events = await OnFilesChanged();

                if (events == null)
                {
                    logger.LogDebug("Result is null, do not publish to eventbus.");
                    return;
                }

                foreach (var @event in events)
                {
                    logger.LogDebug($"Publish event of type {@event.GetType()} to eventbus.");
                    eventBus.Publish(@event);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Something went terribly wrong.");
            }
            finally
            {
                isExportRunning = false;
                lastTrigger = null;
            }
        }
    }
}
