using System.Threading.Tasks;

namespace UntisExportService.Core.Inputs
{
    /// <summary>
    /// Interface for all watchers (can and should be used for DI)
    /// </summary>
    public interface IWatcher<T>
    {
        int SyncThresholdInSeconds { get; set; }

        void Configure(T settings);

        Task TriggerAsync();

        void Start();

        void Stop();
    }
}
