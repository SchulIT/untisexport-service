using System.Collections.Generic;

namespace UntisExportService.Core.Settings.Outputs.Json
{
    public abstract class OutputBase : IOutput
    {
        public abstract string Type { get; }

        public IList<string> Entities { get; set; }
    }
}
