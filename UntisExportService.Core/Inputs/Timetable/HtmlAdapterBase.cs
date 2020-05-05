using SchulIT.UntisExport.Timetable;
using SchulIT.UntisExport.Timetable.Html;
using System.Threading.Tasks;
using UntisExportService.Core.Extensions;
using UntisExportService.Core.Settings.Inputs.Timetable;

namespace UntisExportService.Core.Inputs.Timetable
{
    public abstract class HtmlAdapterBase : IAdapter
    {
        public abstract string SearchPattern { get; }

        protected abstract TimetableType Type { get; }


        private readonly ITimetableExporter exporter;

        public HtmlAdapterBase(ITimetableExporter exporter)
        {
            this.exporter = exporter;
        }

        public Task<TimetableExportResult> GetLessonsAsync(string contents, ITimetableInput settings)
        {
            var htmlSettings = settings as ITimetableInput;

            if(htmlSettings != null)
            {
                var untisSettings = htmlSettings.ToUntis();
                untisSettings.Type = Type;

                return exporter.ParseHtmlAsync(contents, untisSettings);
            }

            return Task.FromResult<TimetableExportResult>(null);
        }
    }
}
