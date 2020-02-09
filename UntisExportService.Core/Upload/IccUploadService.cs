using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SchulIT.UntisExport.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UntisExportService.Core.Settings;

namespace UntisExportService.Core.Upload
{
    public class IccUploadService : IUploadService
    {
        private readonly IEnumerable<IModelStrategy> modelStrategies;

        private readonly ISettingsService settingsService;
        private readonly IHttp httpService;
        private readonly ILogger<IccUploadService> logger;

        public IccUploadService(ISettingsService settingsService, IHttp httpService, IEnumerable<IModelStrategy> modelStrategies, ILogger<IccUploadService> logger)
        {
            this.modelStrategies = modelStrategies;

            this.settingsService = settingsService;
            this.httpService = httpService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns the strategy used to tranform the incoming data into uploadable data
        /// </summary>
        /// <returns></returns>
        private IModelStrategy GetModelStrategy()
        {
            return modelStrategies.FirstOrDefault(x => x.IsSupported(settingsService.Settings));
        }


        /// <summary>
        /// Uploads infotexts to the ICC
        /// </summary>
        /// <param name="infotexts"></param>
        /// <returns></returns>
        public async Task UploadInfotextsAsync(IEnumerable<Infotext> infotexts)
        {
            logger.LogDebug($"Publish {infotexts.Count()} infotext(s) to ICC.");
            var json = await Task.Run(() =>
            {
                var strategy = GetModelStrategy();
                logger.LogDebug($"Converting infotexts using {strategy.GetType().ToString()}");
                return JsonConvert.SerializeObject(strategy.GetInfotexts(infotexts));
            }).ConfigureAwait(false);

            var endpointSettings = settingsService.Settings.Endpoint;

            var response = await httpService.PostJsonAsync(endpointSettings.InfotextsUrl, endpointSettings.ApiKey, json).ConfigureAwait(false);
            await HandleResponseAsync(response).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                logger.LogDebug($"Successfully published infotexts to ICC.");
            }
            else
            {
                logger.LogError($"Publishing infotexts to ICC failed. See log for further information.");
            }
        }

        /// <summary>
        /// Uploads substitutions to the ICC
        /// </summary>
        /// <param name="substitutions"></param>
        /// <returns></returns>
        public async Task UploadSubstitutionsAsync(IEnumerable<Substitution> substitutions)
        {
            logger.LogDebug($"Publish {substitutions.Count()} substitution(s) to ICC.");
            var json = await Task.Run(() =>
            {
                var strategy = GetModelStrategy();
                logger.LogDebug($"Converting substitutions using {strategy.GetType().ToString()}");
                return JsonConvert.SerializeObject(strategy.GetSubstitutions(substitutions));
            }).ConfigureAwait(false);

            var endpointSettings = settingsService.Settings.Endpoint;

            var response = await httpService.PostJsonAsync(endpointSettings.SubstitutionsUrl, endpointSettings.ApiKey, json).ConfigureAwait(false);
            await HandleResponseAsync(response).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                logger.LogDebug($"Successfully published substitutions to ICC.");
            }
            else
            {
                logger.LogError($"Publishing substitutions to ICC failed. See log for further information.");
            }
        }

        public async Task UploadAbsencesAsync(IEnumerable<Absence> absences)
        {
            var strategy = GetModelStrategy();

            if(strategy is LegacyIccModelStrategy)
            {
                logger.LogDebug($"Do not publish absences as they are not supported by legacy ICC.");
                return;
            }

            logger.LogDebug($"Publish {absences.Count()} absences(s) to ICC.");
            var json = await Task.Run(() =>
            {
                    
                logger.LogDebug($"Converting absences using {strategy.GetType().ToString()}");
                return JsonConvert.SerializeObject(strategy.GetAbsences(absences));
            }).ConfigureAwait(false);
            

            var endpointSettings = settingsService.Settings.Endpoint;

            var response = await httpService.PostJsonAsync(endpointSettings.AbsencesUrl, endpointSettings.ApiKey, json).ConfigureAwait(false);
            await HandleResponseAsync(response).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                logger.LogDebug($"Successfully published absences to ICC.");
            }
            else
            {
                logger.LogError($"Publishing absences to ICC failed. See log for further information.");
            }
        }

        private async Task HandleResponseAsync(HttpResponse response)
        {
            logger.LogDebug($"Got response with HTTP {response.StatusCode}");

            if (!response.IsSuccess || settingsService.Settings.IsDebugModeEnabled)
            {
                var filename = "response-" + DateTime.Now.Ticks + ".json";
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var file = Path.Combine(path, filename);

                using (var writer = new StreamWriter(file))
                {
                    try
                    {
                        await writer.WriteAsync(response.Content);
                        logger.LogInformation($"Successfully saved response to {file}.");
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, $"Unable to save response to {file}.");
                    }
                }
            }
        }
    }
}
