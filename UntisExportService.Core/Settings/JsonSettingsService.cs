using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using UntisExportService.Core.FileSystem;

namespace UntisExportService.Core.Settings
{
    public class JsonSettingsService : ISettingsService
    {
        public const string JsonFileName = "settings.json";
        public const string ApplicationName = "UntisExportService";
        public const string ApplicationVendor = "SchulIT";

        public ISettings Settings { get; private set; }

        public event SettingsChangedEventHandler Changed;

        protected void OnChanged(SettingsChangedEventArgs args)
        {
            Changed?.Invoke(this, args);
        }

        private readonly IFileSystemWatcher watcher;
        private readonly ILogger<JsonSettingsService> logger;

        public JsonSettingsService(IFileSystemWatcher watcher, ILogger<JsonSettingsService> logger)
        {
            this.logger = logger;

            LoadSettings(true);

            this.watcher = watcher;
            this.watcher.Changed += OnSettingsFileChanged;
            this.watcher.Path = Path.GetDirectoryName(GetPath());
        }

        /// <summary>
        /// Event handler which is fired in case the settings file is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnSettingsFileChanged(IFileSystemWatcher sender, OnChangedEventArgs args)
        {
            logger.LogDebug("settings.json changed");

            LoadSettings(false);
            OnChanged(new SettingsChangedEventArgs());
        }

        /// <summary>
        /// Loads the settings from the file provided by GetPath()
        /// </summary>
        /// <param name="isInitial">Specifies whether this is an initial load or an load during runtime.</param>
        protected virtual void LoadSettings(bool isInitial)
        {
            var path = GetPath();

            try
            {
                var directory = Path.GetDirectoryName(path);

                if (!Directory.Exists(directory))
                {
                    logger.LogDebug($"Creating directory {directory} as it does not exist.");
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(path))
                {
                    logger.LogDebug("Settings file does not exist, creating a default one.");

                    var jsonSettings = new JsonSettings();
                    using (var writer = new StreamWriter(path))
                    {
                        writer.Write(JsonConvert.SerializeObject(jsonSettings, Formatting.Indented));
                    }

                    logger.LogDebug("Settings file created successfully.");
                }

                logger.LogDebug($"Reading settings from file {path}.");

                using (var reader = new StreamReader(path))
                {
                    var json = reader.ReadToEnd();
                    var settings = JsonConvert.DeserializeObject<JsonSettings>(json);
                    Settings = settings;
                }

                if (isInitial)
                {
                    // Write settings back to create possibly missing new setting items
                    using (var writer = new StreamWriter(path))
                    {
                        writer.Write(JsonConvert.SerializeObject(Settings, Formatting.Indented));
                    }
                }

                logger.LogDebug("Settings read successfully.");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed loading settings from file {path}.");
            }
        }

        protected virtual string GetPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                ApplicationVendor,
                ApplicationName,
                "settings.json"
            );
        }
    }
}
