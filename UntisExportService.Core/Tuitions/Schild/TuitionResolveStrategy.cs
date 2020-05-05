using Microsoft.Extensions.Logging;
using SchulIT.SchildExport;
using SchulIT.SchildExport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UntisExportService.Core.Settings.Tuitions;

namespace UntisExportService.Core.Tuitions.Schild
{
    public class TuitionResolveStrategy : AbstractTuitionResolveStrategy<ISchildTuitionResolver>
    {

        /// <summary>
        /// All grade study groups
        /// </summary>
        private readonly Dictionary<string, StudyGroup> gradeStudyGroups = new Dictionary<string, StudyGroup>();

        /// <summary>
        /// All tuitions
        /// 
        /// Key 1: Grade
        /// Key 2: Subject
        /// </summary>
        private readonly Dictionary<string, List<TuitionStudyGroupTuple>> tuitions = new Dictionary<string, List<TuitionStudyGroupTuple>>();

        private readonly IExporter schildExporter;
        private readonly ILogger<TuitionResolveStrategy> logger;

        public TuitionResolveStrategy(IExporter schildExporter, ILogger<TuitionResolveStrategy> logger)
        {
            this.schildExporter = schildExporter;
            this.logger = logger;
        }

        public override void Initialize(ISchildTuitionResolver inputSetting)
        {
            gradeStudyGroups.Clear();
            tuitions.Clear();

            logger.LogDebug("Initialize SchILD exporter...");

            try
            {
                schildExporter.Configure(inputSetting.ConnectionString, false);
                var schoolInfoTask = schildExporter.GetSchoolInfoAsync();
                schoolInfoTask.Wait();
                var schoolInfo = schoolInfoTask.Result;

                logger.LogInformation($"Current academic year: {schoolInfo.CurrentYear} - section {schoolInfo.CurrentSection}");

                var tuitionsTask = schildExporter.GetTuitionsAsync(Array.Empty<Student>(), schoolInfo.CurrentYear.Value, schoolInfo.CurrentSection.Value);
                tuitionsTask.Wait();
                var tuitions = tuitionsTask.Result;

                logger.LogInformation($"Got {tuitions.Count} tuitions from SchILD");

                var studyGroupsTask = schildExporter.GetStudyGroupsAsync(Array.Empty<Student>(), schoolInfo.CurrentYear.Value, schoolInfo.CurrentSection.Value);
                studyGroupsTask.Wait();
                var studyGroups = studyGroupsTask.Result;

                logger.LogInformation($"Got {studyGroups.Count} studygroups from SchILD");

                // TODO: Optimize this algo
                foreach(var tuition in tuitions.Where(x => x.StudyGroupRef != null && x.SubjectRef != null))
                {
                    var studyGroup = studyGroups.FirstOrDefault(x => x.Id == tuition.StudyGroupRef.Id && x.Name == tuition.StudyGroupRef.Name);

                    if(studyGroup == null)
                    {
                        logger.LogDebug($"Studygroup for tuition with StudyGroupRef.ID={tuition.StudyGroupRef.Id} and StudyGroupRef.Name={tuition.StudyGroupRef.Name} was not found. Ignoring.");
                        continue;
                    }

                    foreach (var grade in studyGroup.Grades)
                    {
                        if (this.tuitions.ContainsKey(grade.Name) != true)
                        {
                            this.tuitions.Add(grade.Name, new List<TuitionStudyGroupTuple>());
                        }

                        var subject = tuition.SubjectRef.Abbreviation;

                        if (inputSetting.GradesWithCourseNameAsSubject.Contains(grade.Name))
                        {
                            subject = studyGroup.Name;
                        }

                        if(inputSetting.SchildToUntisSubjectMap != null && inputSetting.SchildToUntisSubjectMap.ContainsKey(subject))
                        {
                            subject = inputSetting.SchildToUntisSubjectMap[subject];
                        }

                        this.tuitions[grade.Name].Add(new TuitionStudyGroupTuple(tuition, studyGroup, subject));
                    }
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

            var grades = studyGroup.Grades.Select(x => x.Name).Distinct();
            return string.Join("-", studyGroup.Grades.Select(x => x.Name).Distinct().OrderBy(x => x));
        }

        /// <summary>
        /// TODO: Make this configurable
        /// </summary>
        /// <param name="studyGroup"></param>
        /// <returns></returns>
        private string GetStudyGroupId(StudyGroup studyGroup)
        {
            if (studyGroup.Type == StudyGroupType.Course)
            {
                return studyGroup.Id.ToString();
            }

            return GetStudyGroupName(studyGroup);
        }

        public override string ResolveStudyGroup(string grade)
        {
            return gradeStudyGroups.ContainsKey(grade) ? GetStudyGroupId(gradeStudyGroups[grade]) : null;
        }

        public override string ResolveTuition(string grade, string subject, string teacher)
        {
            if(subject == null)
            {
                logger.LogDebug($"Subject must not be empty.");
                return null;
            }

            if(!tuitions.ContainsKey(grade))
            {
                logger.LogDebug($"Grade {grade} does not exist.");
                return null;
            }

            var candidates = tuitions[grade].Where(x => x.Subject == subject);

            if(!candidates.Any())
            {
                logger.LogDebug($"Did not find any tuition for grade {grade}, subject {subject} and teacher {teacher}");
                return null;
            }

            if(candidates.Count() == 1)
            {
                return GetTuitionId(candidates.First().Tuition);
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
                    return GetTuitionId(candidate.Tuition);
                }
            }

            logger.LogDebug($"Did not find any tuition for grade {grade}, subject {subject} and teacher {teacher}");
            return null;
        }

        private string GetTuitionId(Tuition tuition)
        {
            if (tuition.StudyGroupRef?.Id != null)
            {
                return tuition.StudyGroupRef.Id.ToString();
            }

            return $"{tuition.SubjectRef.Abbreviation}-{tuition.StudyGroupRef.Name}";
        }

        private class TuitionStudyGroupTuple : Tuple<Tuition, StudyGroup, string>
        {
            public Tuition Tuition { get { return Item1; } }

            public StudyGroup StudyGroup { get { return Item2; } }

            public string Subject { get { return Item3; } }

            public TuitionStudyGroupTuple(Tuition tuition, StudyGroup studyGroup, string subject)
                : base(tuition, studyGroup, subject) { }
        }
    }
}
