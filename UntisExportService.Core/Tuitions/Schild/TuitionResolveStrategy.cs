using Microsoft.Extensions.Logging;
using SchulIT.SchildExport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UntisExportService.Core.External.Schild;
using UntisExportService.Core.Settings.Tuitions;

namespace UntisExportService.Core.Tuitions.Schild
{
    public class TuitionResolveStrategy : AbstractTuitionResolveStrategy<ISchildTuitionResolver>
    {

        /// <summary>
        /// All grade study groups
        /// </summary>
        private readonly Dictionary<string, StudyGroup> gradeStudyGroupsCache = new Dictionary<string, StudyGroup>();

        /// <summary>
        /// All tuitions
        /// 
        /// Key 1: Grade
        /// Key 2: Subject
        /// </summary>
        private readonly Dictionary<string, List<TuitionStudyGroupTuple>> tuitionsCache = new Dictionary<string, List<TuitionStudyGroupTuple>>();

        private readonly ISchildAdapter schildAdapter;
        private readonly ILogger<TuitionResolveStrategy> logger;

        public TuitionResolveStrategy(ISchildAdapter schildAdapter, ILogger<TuitionResolveStrategy> logger)
        {
            this.schildAdapter = schildAdapter;
            this.logger = logger;
        }

        public override void Initialize(ISchildTuitionResolver inputSetting)
        {
            gradeStudyGroupsCache.Clear();
            tuitionsCache.Clear();

            logger.LogDebug("Initialize SchILD exporter...");

            try
            {
                var schildExporter = schildAdapter.GetExporter();
                var schoolInfoTask = schildExporter.GetSchoolInfoAsync();
                schoolInfoTask.Wait();
                var schoolInfo = schoolInfoTask.Result;

                logger.LogDebug($"Current academic year: {schoolInfo.CurrentYear} - section {schoolInfo.CurrentSection}");

                var tuitions = schildAdapter.LoadTuitions(schoolInfo.CurrentYear.Value, schoolInfo.CurrentSection.Value);

                foreach(var kv in tuitions)
                {
                    tuitionsCache.Add(kv.Key, kv.Value);
                }

                var studyGroups = schildAdapter.GetGradeStudyGroups(schoolInfo.CurrentYear.Value, schoolInfo.CurrentSection.Value);

                foreach(var kv in studyGroups)
                {
                    gradeStudyGroupsCache.Add(kv.Key, kv.Value);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to load tuitions from SchILD database. Study group resolving will fail.");
            }
        }

        /// <summary>
        /// TODO: Make this configurable
        /// </summary>
        /// <param name="studyGroup"></param>
        /// <returns></returns>
        private string GetStudyGroupName(StudyGroup studyGroup)
        {
            if (studyGroup.Type == StudyGroupType.Course)
            {
                return studyGroup.Name;
            }

            return GetStudyGroupId(studyGroup);
        }

        public override string ResolveStudyGroup(string grade)
        {
            if (grade == null)
            {
                logger.LogDebug($"Grade must not be empty.");
                return null;
            }

            return gradeStudyGroupsCache.ContainsKey(grade) ? GetStudyGroupId(gradeStudyGroupsCache[grade]) : null;
        }

        public override string ResolveStudyGroup(string grade, string subject, string teacher)
        {
            if (grade == null)
            {
                logger.LogDebug($"Grade must not be empty.");
                return null;
            }

            if (subject == null)
            {
                logger.LogDebug($"Subject must not be empty.");
                return null;
            }

            if (!tuitionsCache.ContainsKey(grade))
            {
                logger.LogDebug($"Grade {grade} does not exist.");
                return null;
            }

            var candidates = tuitionsCache[grade].Where(x => x.Subject == subject);

            if (!candidates.Any())
            {
                logger.LogDebug($"Did not find any tuition for grade {grade}, subject {subject} and teacher {teacher}");
                return null;
            }

            if (candidates.Count() == 1)
            {
                return GetStudyGroupId(candidates.First().StudyGroup);
            }

            var teachers = candidates.Select(x => x.Tuition.TeacherRef.Acronym).ToList();

            logger.LogDebug($"Possible teachers are: {string.Join(", ", teachers)}");

            if (string.IsNullOrEmpty(teacher))
            {
                logger.LogDebug($"Ambiguous tuition found for grade {grade} and subject {subject}. Teacher needs to be specified.");
                return null;
            }

            foreach (var candidate in candidates.Where(x => x.Tuition.TeacherRef != null))
            {
                if (candidate.Tuition.TeacherRef.Acronym == teacher)
                {
                    return GetTuitionId(candidate.Tuition, candidate.StudyGroup);
                }
            }

            logger.LogDebug($"Did not find any tuition for grade {grade}, subject {subject} and teacher {teacher}");
            return null;
        }

        public override string ResolveTuition(string grade, string subject, string teacher)
        {
            if(grade == null)
            {
                logger.LogDebug($"Grade must not be empty.");
                return null;
            }

            if(subject == null)
            {
                logger.LogDebug($"Subject must not be empty.");
                return null;
            }

            if(!tuitionsCache.ContainsKey(grade))
            {
                logger.LogDebug($"Grade {grade} does not exist.");
                return null;
            }

            var candidates = tuitionsCache[grade].Where(x => x.Subject == subject);

            if(!candidates.Any())
            {
                logger.LogDebug($"Did not find any tuition for grade {grade}, subject {subject} and teacher {teacher}");
                return null;
            }

            if(candidates.Count() == 1)
            {
                return GetTuitionId(candidates.First().Tuition, candidates.First().StudyGroup);
            }

            if(string.IsNullOrEmpty(teacher))
            {
                logger.LogDebug($"Ambiguous tuition found for grade {grade} and subject {subject}. Teacher needs to be specified.");
                return null;
            }

            foreach(var candidate in candidates.Where(x => x.Tuition.TeacherRef != null))
            {
                if(candidate.Tuition.TeacherRef.Acronym == teacher)
                {
                    return GetTuitionId(candidate.Tuition, candidate.StudyGroup);
                }
            }

            logger.LogDebug($"Did not find any tuition for grade {grade}, subject {subject} and teacher {teacher}");
            return null;
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
