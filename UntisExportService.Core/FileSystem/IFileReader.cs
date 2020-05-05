using System.Text;
using System.Threading.Tasks;

namespace UntisExportService.Core.FileSystem
{
    public interface IFileReader
    {
        Task<string> GetContentsAsync(string path, Encoding encoding);
    }
}
