using Microsoft.Extensions.Logging;
using SchulIT.SchildExport;
using SchulIT.SchildExport.Linq;
using SchulIT.SchildExport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UntisExportService.Core.Extensions;
using UntisExportService.Core.Settings;
using UntisExportService.Core.Settings.External.Schild;

namespace UntisExportService.Core.External.Schild
{
    public class SchildAdapter : ISchildAdapter
    {
        private readonly IExporter exporter;
        private readonly ISettingsService settingsService;
        private readonly ILogger<SchildAdapter> logger;

        public SchildAdapter(IExporter exporter, ISettingsService settingsService, ILogger<SchildAdapter> logger)
        {
            this.exporter = exporter;
            this.settingsService = settingsService;
            this.logger = logger;
        }

        public IExporter GetExporter()
        {
            var settings = settingsService.Settings.ExternalServices.FirstOrDefault(x => x is ISchildSettings);

            if(settings == null)
            {
                logger.LogError("Cannot connect to SchILD as no such service was configured in the 'external' section.");
                throw new Exception("Cannot connect to SchILD as no such service was configured in the 'external' section.");
            }

            var schildSettings = settings as ISchildSettings;
            logger.LogDebug("Configure SchILD service");
            exporter.Configure(schildSettings.ConnectionString, false);

            return exporter;
        }

        public Dictionary<string, StudyGroup> GetGradeStudyGroups(short year, short section)
        {
            logger.LogDebug($"Getting grade study groups ({year}/{section})...");

            var gradeStudyGroups = new Dictionary<string, StudyGroup>();

            try
            {
                logger.LogDebug("Get students...");
                var studentsTask = exporter.GetStudentsAsync(year, section);
                studentsTask.Wait();
                var students = studentsTask.Result;
                logger.LogDebug($"Got {students.Count} students from SchILD");

                logger.LogDebug("Get study groups...");
                var studyGroupsTask = exporter.GetStudyGroupsAsync(students, year, section);
                studyGroupsTask.Wait();
                var studyGroups = studyGroupsTask.Result;

                logger.LogDebug($"Got {studyGroups.Count} studygroups from SchILD");

                foreach (var studyGroup in studyGroups.Where(x => x.Type == StudyGroupType.Grade))
                {
                    gradeStudyGroups.Add(studyGroup.Name, studyGroup);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to load grade study groups from SchILD database. Study group resolving will fail.");
            }

            return gradeStudyGroups;
        }

        public Dictionary<string, List<TuitionStudyGroupTuple>> LoadTuitions(short year, short section)
        {
            var settings = settingsService.Settings.ExternalServices.FirstOrDefault(x => x is ISchildSettings) as ISchildSettings;

            if (settings == null)
            {
                logger.LogError("Cannot connect to SchILD as no such service was configured in the 'external' section.");
                throw new Exception("Cannot connect to SchILD as no such service was configured in the 'external' section.");
            }

            logger.LogDebug("Getting tuitions ({year}/{section})...");

            var result = new Dictionary<string, List<TuitionStudyGroupTuple>>();

            try
            {
                logger.LogDebug("Get students...");
                var studentsTask = exporter.GetStudentsAsync(year, section);
                studentsTask.Wait();
                var students = studentsTask.Result;
                logger.LogDebug($"Got {students.Count} students from SchILD");

                var tuitionsTask = exporter.GetTuitionsAsync(students, year, section);
                tuitionsTask.Wait();
                var tuitions = tuitionsTask.Result;

                logger.LogInformation($"Got {tuitions.Count} tuitions from SchILD");

                var studyGroupsTask = exporter.GetStudyGroupsAsync(students, year, section);
                studyGroupsTask.Wait();
                var studyGroups = studyGroupsTask.Result;

                logger.LogInformation($"Got {studyGroups.Count} studygroups from SchILD");

                // TODO: Optimize this algo
                foreach (var tuition in tuitions.Where(x => x.StudyGroupRef != null && x.SubjectRef != null))
                {
                    var studyGroup = studyGroups.FirstOrDefault(x => x.Id == tuition.StudyGroupRef.Id && x.Name == tuition.StudyGroupRef.Name);

                    if (studyGroup == null)
                    {
                        logger.LogDebug($"Studygroup for tuition with StudyGroupRef.ID={tuition.StudyGroupRef.Id} and StudyGroupRef.Name={tuition.StudyGroupRef.Name} was not found. Ignoring.");
                        continue;
                    }

                    studyGroup.Grades = studyGroup.Grades.WhereIsVisible().ToList();

                    foreach (var grade in studyGroup.Grades)
                    {
                        if (result.ContainsKey(grade.Name) != true)
                        {
                            result.Add(grade.Name, new List<TuitionStudyGroupTuple>());
                        }

                        var subject = tuition.SubjectRef.Abbreviation;

                        if (settings.GradesWithCourseNameAsSubject.Contains(grade.Name))
                        {
                            subject = studyGroup.Name;
                        }

                        var courseRule = settings.SubjectConversationRules.FirstOrDefault(x => x.IsCourse && x.ExternalSubject == studyGroup.Name && x.Grades.MatchesAny(grade.Name));

                        if (courseRule != null)
                        {
                            logger.LogDebug($"Found a conversion rule for study group {studyGroup.Name} (grade {grade.Name}): Untis subject is {courseRule.UntisSubject}");
                            subject = courseRule.UntisSubject;
                        }

                        var subjectRule = settings.SubjectConversationRules.FirstOrDefault(x => !x.IsCourse && x.ExternalSubject == tuition.SubjectRef.Abbreviation && x.Grades.MatchesAny(grade.Name));

                        if (subjectRule != null)
                        {
                            logger.LogDebug($"Found a conversion rule for subject {tuition.SubjectRef.Abbreviation} (grade {grade.Name}): Untis subject is {subjectRule.UntisSubject}");
                            subject = courseRule.UntisSubject;
                        }

                        result[grade.Name].Add(new TuitionStudyGroupTuple(tuition, studyGroup, subject));
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to load tuitions from SchILD database. Study group resolving will fail.");
            }

            return result;
        }
    }
}
