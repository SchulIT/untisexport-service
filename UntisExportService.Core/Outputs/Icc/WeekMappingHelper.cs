using System.Collections.Generic;

namespace UntisExportService.Core.Outputs.Icc
{
    public class WeekMappingHelper
    {
        private const int WeekStart = 1;
        private const int WeekEnd = 53;

        public Dictionary<int, string> ComputeMapping(Dictionary<int, string> baseMapping)
        {
            var mapping = new Dictionary<int, string>();
            var mod = baseMapping.Count;

            for(int week = WeekStart; week <= WeekEnd; week++)
            {
                mapping.Add(week, baseMapping[week % mod]);
            }

            return mapping;
        }
    }
}
