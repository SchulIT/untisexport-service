using System.Threading.Tasks;

namespace UntisExportService.Core
{
    public interface IExportService
    {
        void Start();

        void Start(params InputType[] types);

        void Stop();

        void Configure();

        Task TriggerAsync(InputType type);
    }
}
