using SchulIT.UntisExport.Exams.Html;

namespace UntisExportService.Core.Settings.Inputs.Exams
{
    public interface IHtmlExamInput : IExamInput, IHtmlInput
    {
        string DateTimeFormat { get; }

        IHtmlExamColumns Columns { get; }
    }
}
