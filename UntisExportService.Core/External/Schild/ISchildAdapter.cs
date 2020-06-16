using SchulIT.SchildExport;
using SchulIT.SchildExport.Models;
using System.Collections.Generic;

namespace UntisExportService.Core.External.Schild
{
    public interface ISchildAdapter
    {
        IExporter GetExporter();

        public Dictionary<string, List<TuitionStudyGroupTuple>> LoadTuitions(short year, short section);

        public Dictionary<string, StudyGroup> GetGradeStudyGroups(short year, short section);
    }
}
