using SchulIT.UntisExport.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UntisExportService.Core.Upload
{
    public interface IUploadService
    {
        Task UploadSubstitutionsAsync(IEnumerable<Substitution> substitutions);

        Task UploadInfotextsAsync(IEnumerable<Infotext> infotexts);
    }
}
