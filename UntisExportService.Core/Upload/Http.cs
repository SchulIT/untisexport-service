using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UntisExportService.Core.Upload
{
    public class Http : IHttp
    {
        private ILogger<Http> logger;

        public Http(ILogger<Http> logger)
        {
            this.logger = logger;
        }

        public async Task<HttpResponse> PostJsonAsync(string endpoint, string apiKey, string json)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                client.DefaultRequestHeaders.Add("X-Token", apiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(endpoint, content).ConfigureAwait(false);

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if(!response.IsSuccessStatusCode)
                {
                    logger.LogError($"Response code did not indicate success. Got HTTP {response.StatusCode}");
                }

                return new HttpResponse(response.IsSuccessStatusCode, (int)response.StatusCode, responseContent);
            }
        }
    }
}
