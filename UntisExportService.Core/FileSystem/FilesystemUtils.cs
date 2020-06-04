using DotNet.Globbing;
using System.Collections.Generic;
using System.IO;

namespace UntisExportService.Core.FileSystem
{
    public static class FilesystemUtils
    {
        public static string[] GetFiles(string path, string pattern)
        {
            var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
            var result = new List<string>();

            var glob = Glob.Parse(pattern);

            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(path, file);

                if (glob.IsMatch(relativePath))
                {
                    result.Add(file);
                }
            }

            return result.ToArray();
        }
    }
}
