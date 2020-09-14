using Microsoft.Extensions.Logging;
using SchulIT.SchildExport.Models;
using SchulIT.UntisExport.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using UntisExportService.Core.External.Schild;
using UntisExportService.Core.Settings.ExamWriters.Schild;

namespace UntisExportService.Core.ExamWriters.Schild
{
    public class SchildExamWritersResolveStrategy : IExamWritersResolveStrategy
    {
        private ISchildExamWritersResolver settings;

        /// <summary>
        /// Key: year-section
        /// Value: all tuitions of this section
        /// </summary>
        private readonly Dictionary<string, List<TuitionStudyGroupTuple>> tuitionCache = new Dictionary<string, List<TuitionStudyGroupTuple>>();
        private readonly Dictionary<string, Student> studentCache = new Dictionary<string, Student>();

        private readonly ISchildAdapter schildAdapter;
        private readonly ILogger<SchildExamWritersResolveStrategy> logger;

        public SchildExamWritersResolveStrategy(ISchildAdapter schildAdapter, ILogger<SchildExamWritersResolveStrategy> logger)
        {
            this.schildAdapter = schildAdapter;
            this.logger = logger;
        }

        public void Initialize(Settings.ExamWriters.IExamWritersResolver inputSetting)
        {
            settings = inputSetting as ISchildExamWritersResolver;

            var exporter = schildAdapter.GetExporter();
            var grades = settings.Rules.SelectMany(x => x.Grades).Distinct().ToList();

            tuitionCache.Clear();

            logger.LogDebug($"Getting study groups from SchILD...");

            foreach(var section in settings.Sections)
            {
                var key = $"{section.SchoolYear}-{section.Section}";
                logger.LogDebug($"Getting study groups from SchILD ({section.SchoolYear}/{section.Section})...");
                var tuitions = schildAdapter.LoadTuitions(section.SchoolYear, section.Section);

                tuitionCache.Add(key, new List<TuitionStudyGroupTuple>());

                foreach(var kv in tuitions)
                {
                    if(grades.Contains(kv.Key))
                    {
                        tuitionCache[key].AddRange(kv.Value);
                    }
                }
            }

            logger.LogDebug("Getting students from SchILD...");
            var studentTask = schildAdapter.GetExporter().GetStudentsAsync();
            studentTask.Wait();

            foreach (var student in studentTask.Result)
            {
                if (!studentCache.ContainsKey(student.Id.ToString()))
                {
                    studentCache.Add(student.Id.ToString(), student);
                }
            }
        }

        private SchildSection GetSectionForDate(DateTime date)
        {
            foreach(var section in settings.Sections)
            {
                if(section.Start <= date && section.End >= date)
                {
                    return section;
                }
            }

            return null;
        } 

        public List<string> Resolve(string tuition, Exam exam, string start, string end)
        {
            var date = exam.Date;
            var students = new List<string>();
            var section = GetSectionForDate(date);

            if(section == null)
            {
                logger.LogError($"Cannot find any SchILD section for date {date.ToShortDateString()}. Students will be empty.");
                return students;
            }

            var key = $"{section.SchoolYear}-{section.Section}";

            if(!tuitionCache.ContainsKey(key))
            {
                logger.LogError($"Did not find any tuitions for {key}. Students will be emtpy.");
                return students;
            }

            var match = tuitionCache[key].FirstOrDefault(x => GetTuitionId(x.Tuition, x.StudyGroup) == tuition);

            if(match == null)
            {
                logger.LogError($"Did not find any tuitions for {tuition}. Students will be emtpy.");
                return students;
            }

            foreach(var membership in match.StudyGroup.Memberships)
            {
                var student = studentCache.ContainsKey(membership.Student.Id.ToString()) ? studentCache[membership.Student.Id.ToString()] : null;
                var candidate = settings.Rules.FirstOrDefault(x => x.Grades.Contains(membership.Grade) && x.Sections.Contains(section.Section) && x.Types.Contains(membership.Type));

                if (candidate == null)
                {
                    logger.LogDebug($"Did not find any rule for student {membership.Student.Id} (Grade: {membership.Grade}, Type: {membership.Type}, Section: {section.Section}. Ignore student.");
                }
                else if(!string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end) && student != null && (String.Compare(student.Lastname.ToUpper(), start.ToUpper()) < 0 || student.Lastname.ToUpper().Substring(0, end.Length) != end.ToUpper()))
                {
                    logger.LogDebug($"Ignore student with lastname {student.Lastname} because {start} <= {student.Lastname} <= {end} is not true.");
                }
                else
                {
                    students.Add(membership.Student.Id.ToString());
                }

            }

            return students;
        }

        public bool Supports(Settings.ExamWriters.IExamWritersResolver inputSetting)
        {
            return inputSetting is ISchildExamWritersResolver;
        }

        private string GetStudyGroupId(StudyGroup studyGroup)
        {
            var grades = studyGroup.Grades.Select(x => x.Name).Distinct().OrderBy(x => x);
            var gradesString = string.Join("-", grades);

            if (studyGroup.Type == StudyGroupType.Course)
            {
                return $"{gradesString}-{studyGroup.Name}";
            }

            return gradesString;
        }

        private string GetTuitionId(Tuition tuition, StudyGroup studyGroup)
        {
            if (studyGroup?.Id != null)
            {
                return GetStudyGroupId(studyGroup);
            }

            return $"{tuition.SubjectRef.Abbreviation}-{tuition.StudyGroupRef.Name}";
        }
    }
}
