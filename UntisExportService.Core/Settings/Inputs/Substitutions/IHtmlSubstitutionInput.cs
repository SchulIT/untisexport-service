using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Inputs.Substitutions
{
    public interface IHtmlSubstitutionInput : ISubstitutionInput, IHtmlInput
    {
        IHtmlSubstitutionColumns ColumnSettings { get; }

        IHtmlSubstitutionOptions Options { get; }

        IHtmlAbsenceSettings AbsenceSettings { get; }

        IHtmlFreeLessonSettings FreeLessonSettings { get; }

        List<string> TypesWithRemovedReplacementColumns { get; }
    }
}
