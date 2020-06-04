using SchulIT.SchildExport;

namespace UntisExportService.Core.External.Schild
{
    public interface ISchildAdapter
    {
        IExporter GetExporter();
    }
}
