using DotNet.Globbing;
using SchulIT.UntisExport.Timetable;
using SchulIT.UntisExport.Timetable.Html;
using System.Collections.Generic;
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

        private List<string> GetWhitelist(ITimetableInput timetableInput)
        {
            switch (Type)
            {
                case TimetableType.Grade:
                    return timetableInput.Grades;

                case TimetableType.Subject:
                    return timetableInput.Subjects;
            }

            return new List<string>();
        }

        public bool IsMarkedToExport(string objective, ITimetableInput timetableInput)
        {
            var whitelist = GetWhitelist(timetableInput);

            foreach (var pattern in whitelist)
            {
                var glob = Glob.Parse(pattern);
                if (glob.IsMatch(objective))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
