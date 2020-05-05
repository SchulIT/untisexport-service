using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Outputs
{
    public interface IWeekMapping
    {
        Dictionary<int, string> Weeks { get; }

        /// <summary>
        /// Whether or not to use the week modulo (so you do not have to 
        /// specify all weeks)
        /// </summary>
        bool UseWeekModulo { get; }
    }
}
