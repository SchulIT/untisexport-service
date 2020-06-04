using SchulIT.UntisExport.Timetable;
using System.Threading.Tasks;
using UntisExportService.Core.Settings.Inputs.Timetable;

namespace UntisExportService.Core.Inputs.Timetable
{
    public interface IAdapter
    {
        string SearchPattern { get; }

        Task<TimetableExportResult> GetLessonsAsync(string contents, ITimetableInput timetableInput);

        bool IsMarkedToExport(string objective, ITimetableInput timetableInput);
    }
}
