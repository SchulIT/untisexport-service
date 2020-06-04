using DotNet.Globbing;
using System.Collections.Generic;

namespace UntisExportService.Core.Extensions
{
    public static class CollectionEx
    {

        /// <summary>
        /// Checks whether subject matches a list of glob patterns.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static bool MatchesAny(this IEnumerable<string> items, string subject)
        {
            foreach(var pattern in items)
            {
                var glob = Glob.Parse(pattern);
                
                if(glob.IsMatch(subject))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
