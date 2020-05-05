using System.Threading.Tasks;

namespace UntisExportService.Outputs
{
    public interface IHttp
    {
        Task<HttpResponse> PostJsonAsync(string endpoint, string apiKey, string json);
    }
}
