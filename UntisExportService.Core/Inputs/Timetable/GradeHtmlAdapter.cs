using SchulIT.UntisExport.Timetable.Html;

namespace UntisExportService.Core.Inputs.Timetable
{
    public class GradeHtmlAdapter : HtmlAdapterBase
    {
        public override string SearchPattern { get { return "Ex_K_HTML_*.htm"; } }

        protected override TimetableType Type { get { return TimetableType.Grade; } }

        public GradeHtmlAdapter(ITimetableExporter exporter)
            : base(exporter)
        {

        }
    }
}
