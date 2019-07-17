using System.Threading.Tasks;

namespace UntisExportService.Core.Upload
{
    public interface IHttp
    {
        Task<HttpResponse> PostJsonAsync(string endpoint, string apiKey, string json);
    }
}
