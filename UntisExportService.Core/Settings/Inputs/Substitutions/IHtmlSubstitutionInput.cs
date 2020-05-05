namespace UntisExportService.Core.Settings.Inputs.Substitutions
{
    public interface IHtmlSubstitutionInput : ISubstitutionInput, IHtmlInput
    {
        IHtmlSubstitutionColumns ColumnSettings { get; }

        IHtmlSubstitutionOptions Options { get; }

        IHtmlAbsenceSettings AbsenceSettings { get; }
    }
}
