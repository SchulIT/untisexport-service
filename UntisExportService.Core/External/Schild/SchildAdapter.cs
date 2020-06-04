using Microsoft.Extensions.Logging;
using SchulIT.SchildExport;
using System;
using System.Linq;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Settings.External.Schild;

namespace UntisExportService.Core.External.Schild
{
    public class SchildAdapter : ISchildAdapter
    {
        private readonly IExporter exporter;
        private readonly ISettingsService settingsService;
        private readonly ILogger<SchildAdapter> logger;

        public SchildAdapter(IExporter exporter, ISettingsService settingsService, ILogger<SchildAdapter> logger)
        {
            this.exporter = exporter;
            this.settingsService = settingsService;
            this.logger = logger;
        }

        public IExporter GetExporter()
        {
            var settings = settingsService.Settings.ExternalServices.FirstOrDefault(x => x is ISchildSettings);

            if(settings == null)
            {
                logger.LogError("Cannot connect to SchILD as no such service was configured in the 'external' section.");
                throw new Exception("Cannot connect to SchILD as no such service was configured in the 'external' section.");
            }

            var schildSettings = settings as ISchildSettings;
            logger.LogDebug("Configure SchILD service");
            exporter.Configure(schildSettings.ConnectionString, false);

            return exporter;
        }
    }
}
