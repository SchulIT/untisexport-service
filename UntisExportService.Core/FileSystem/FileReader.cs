using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UntisExportService.Core.FileSystem
{
    public class FileReader : IFileReader
    {
        public async Task<string> GetContentsAsync(string path, Encoding encoding)
        {
            using (var streamReader = new StreamReader(path, encoding))
            {
                return await streamReader.ReadToEndAsync().ConfigureAwait(false);
            }
        }
    }
}
