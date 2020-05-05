using SchulIT.UntisExport.Timetable.Html;

namespace UntisExportService.Core.Inputs.Timetable
{
    public class SubjectHtmlAdapter : HtmlAdapterBase
    {
        public override string SearchPattern { get { return "Ex_F_HTML_*.htm"; } }

        protected override TimetableType Type { get { return TimetableType.Subject; } }

        public SubjectHtmlAdapter(ITimetableExporter exporter)
            : base(exporter)
        {

        }
    }
}
