using SchulIT.UntisExport.Timetable.Html;

namespace UntisExportService.Core.Inputs.Timetable
{
    public class GradeHtmlAdapter : HtmlAdapterBase
    {
        public override string SearchPattern { get { return "**/c*.htm"; } }

        protected override TimetableType Type { get { return TimetableType.Grade; } }

        public GradeHtmlAdapter(ITimetableExporter exporter)
            : base(exporter)
        {

        }
    }
}
