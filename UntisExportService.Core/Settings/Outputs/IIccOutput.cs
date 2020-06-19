using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Outputs
{
    public interface IIccOutput : IOutput
    {
        IEndpointSettings Endpoint { get; }

        Dictionary<string, string> TimetablePeriodMapping { get; }

        IWeekMapping WeekMapping { get; }

        string SupervisionPeriod { get; }

        string SetNameAsIdPattern { get; }

        string SetNoStudentsPattern { get; }

        string StudentSubsetPattern { get; }
    }
}
