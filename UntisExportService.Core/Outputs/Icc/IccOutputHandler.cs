using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SchulIT.IccImport;
using SchulIT.IccImport.Models;
using SchulIT.IccImport.Response;
using SchulIT.SchildExport.Models;
using SchulIT.UntisExport.Exams;
using SchulIT.UntisExport.Substitutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UntisExportService.Core.ExamWriters;
using UntisExportService.Core.Inputs.Exams;
using UntisExportService.Core.Inputs.Rooms;
using UntisExportService.Core.Inputs.Substitutions;
using UntisExportService.Core.Inputs.Supervisions;
using UntisExportService.Core.Inputs.Timetable;
using UntisExportService.Core.Inputs.Tuitions;
using UntisExportService.Core.Settings.Outputs;
using UntisExportService.Core.Tuitions;

namespace UntisExportService.Core.Outputs.Icc
{
    public class IccOutputHandler : OutputHandlerBase<IIccOutput>
    {
        public override bool CanHandleAbsences { get { return true; } }

        public override bool CanHandleExams { get { return true; } }

        public override bool CanHandleInfotexts { get { return true; } }

        public override bool CanHandleRooms { get { return true; } }

        public override bool CanHandleSubstitutions { get { return true; } }

        public override bool CanHandleSupervisions { get { return true; } }

        public override bool CanHandleTimetable { get { return true; } }

        public override bool CanHandleTuitions { get { return false; } }

        public override bool CanHandleFreeLessons { get { return true; } }

        private readonly WeekMappingHelper weekMappingHelper;
        private readonly IExamWritersResolver examWritersResolver;
        private readonly ITuitionResolver tuitionResolver;
        private readonly IIccImporter iccImporter;
        private readonly ILogger<IccOutputHandler> logger;

        public IccOutputHandler(IExamWritersResolver examWritersResolver, ITuitionResolver tuitionResolver, IIccImporter iccImporter, WeekMappingHelper weekMappingHelper, ILogger<IccOutputHandler> logger)
        {
            this.weekMappingHelper = weekMappingHelper;
            this.examWritersResolver = examWritersResolver;
            this.tuitionResolver = tuitionResolver;
            this.iccImporter = iccImporter;
            this.logger = logger;
        }

        private void Configure(IIccOutput outputSettings)
        {
            iccImporter.Token = outputSettings.Endpoint.Token;
            iccImporter.BaseUrl = outputSettings.Endpoint.Url;
        }

        private Task HandleResponseAsync(IResponse response)
        {
            if (response is ErrorResponse)
            {
                logger.LogError($"Upload was not successful.");
                logger.LogError(response.ResponseBody);
            }
            else if (response is ImportResponse)
            {
                var importResponse = response as ImportResponse;
                logger.LogInformation($"Import successful: {importResponse.AddedCount} items added, {importResponse.UpdatedCount} items updated, {importResponse.RemovedCount} items removed and {importResponse.IgnoredEntities.Count} items ignored.");

                if(importResponse.IgnoredEntities.Count > 0)
                {
                    logger.LogInformation(JsonConvert.SerializeObject(importResponse.IgnoredEntities));
                }
            }
            else if (response is SuccessReponse)
            {
                logger.LogInformation("Upload successful.");
            }

            return Task.CompletedTask;
        }

        private string ConvertObjectiveTypeToString(Absence.ObjectiveType type)
        {
            switch(type)
            {
                case Absence.ObjectiveType.StudyGroup:
                    return "study_group";

                case Absence.ObjectiveType.Teacher:
                    return "teacher";

                case Absence.ObjectiveType.Room:
                    return "room";
            }

            return null;
        }

        protected override async Task HandleAbsenceEvent(AbsenceEvent @event, IIccOutput outputSettings)
        {
            Configure(outputSettings);
            tuitionResolver.Initialize();

            var absences = @event.Absences.Select(absence =>
            {
                var objective = absence.Objective;

                if(absence.Type == Absence.ObjectiveType.StudyGroup)
                {
                    objective = tuitionResolver.ResolveStudyGroup(absence.Objective);
                }

                return new AbsenceData
                {
                    Date = absence.Date,
                    Objective = absence.Objective,
                    Type = ConvertObjectiveTypeToString(absence.Type),
                    LessonStart = absence.LessonStart,
                    LessonEnd = absence.LessonEnd
                };
            });

            var response = await iccImporter.ImportAbsencesAsync(absences.ToList());
            await HandleResponseAsync(response);
        }

        protected override async Task HandleExamEvent(ExamEvent @event, IIccOutput outputSettings)
        {
            Configure(outputSettings);
            tuitionResolver.Initialize();
            examWritersResolver.Initialize();

            Regex regexUseNameAsId = null;
            Regex regexNoStudents = null;
            Regex regexStudentSubset = null;

            var section = new List<Tuple<string, string>>();

            if(!string.IsNullOrEmpty(outputSettings.SetNameAsIdPattern))
            {
                regexUseNameAsId = new Regex(outputSettings.SetNameAsIdPattern);
            }

            if(!string.IsNullOrEmpty(outputSettings.SetNoStudentsPattern))
            {
                regexNoStudents = new Regex(outputSettings.SetNoStudentsPattern);
            }

            if(!string.IsNullOrEmpty(outputSettings.StudentSubsetPattern))
            {
                regexStudentSubset = new Regex(outputSettings.StudentSubsetPattern);
            }

            var examIds = new List<string>();

            var exams = @event.Exams.Select(exam =>
            {
                var tuitions = new List<string>();

                foreach(var course in exam.Courses)
                {
                    foreach(var grade in exam.Grades)
                    {
                        var tuition = tuitionResolver.ResolveTuition(grade, course, null);

                        if (tuition != null)
                        {
                            tuitions.Add(tuition);
                        }
                        else
                        {
                            logger.LogDebug($"Tuition for grade {grade} and course {course} not found.");
                        }
                    }
                }

                var id = GetOrComputeId(exam);

                if(regexUseNameAsId != null && regexUseNameAsId.IsMatch(exam.Name))
                {
                    id = exam.Name;
                }

                var students = new List<string>();

                if(regexNoStudents == null || !regexNoStudents.IsMatch(exam.Name))
                {
                    var examWriters = new List<string>();

                    if (regexStudentSubset != null && regexStudentSubset.IsMatch(exam.Name))
                    {
                        foreach (Match match in regexStudentSubset.Matches(exam.Name))
                        {
                            if (match.Groups.Count < 3)
                            {
                                logger.LogError($"Matching students subset failed as {match.Groups.Count} groups were matched, 3 expected.");
                            }
                            else
                            {
                                var start = match.Groups[1].Value;
                                var end = match.Groups[2].Value;
                                logger.LogDebug($"Only find students between {start} and {end}.");
                                foreach (var tuition in tuitions)
                                {
                                    examWriters.AddRange(examWritersResolver.ResolveStudents(tuition, exam, start, end));
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var tuition in tuitions)
                        {
                            examWriters.AddRange(examWritersResolver.ResolveStudents(tuition, exam, null, null));
                        }
                    }

                    examWriters = examWriters.Distinct().ToList();
                    students.AddRange(examWriters);
                }

                var originalId = id;
                var roomAdded = false;
                var number = 1;
                while(examIds.Contains(id))
                {
                    if (roomAdded == false && exam.Rooms.Count > 0)
                    {
                        id += exam.Rooms.First();
                    }
                    else
                    {
                        id = originalId + (number++);
                    }
                }

                examIds.Add(id);

                return new ExamData
                {
                    Id = id,
                    Date = exam.Date,
                    LessonStart = exam.LessonStart,
                    LessonEnd = exam.LessonEnd,
                    Description = exam.Remark,
                    Rooms = exam.Rooms.ToList(),
                    Supervisions = exam.Supervisions.ToList(),
                    Tuitions = tuitions,
                    Students = students
                };
            });



            var response = await iccImporter.ImportExamsAsync(exams.ToList());
            await HandleResponseAsync(response);
        }

        protected override async Task HandleInfotextEvent(InfotextEvent @event, IIccOutput outputSettings)
        {
            Configure(outputSettings);

            var infotexts = @event.Infotexts.Select(infotext =>
            {
                return new InfotextData
                {
                    Content = infotext.Text,
                    Date = infotext.Date
                };
            });

            var response = await iccImporter.ImportInfotextsAsync(infotexts.ToList());
            await HandleResponseAsync(response);
        }

        protected override async Task HandleRoomEvent(RoomEvent @event, IIccOutput outputSettings)
        {
            Configure(outputSettings);

            var rooms = @event.Rooms.Select(room =>
            {
                return new RoomData
                {
                    Id = room.Name,
                    Capacity = room.Capacity,
                    Description = GetStringOrNull(room.LongName),
                    Name = room.Name
                };
            });

            var response = await iccImporter.ImportRoomsAsync(rooms.ToList());
            await HandleResponseAsync(response);
        }

        protected override async Task HandleSubstitutionEvent(SubstitutionEvent @event, IIccOutput outputSettings)
        {
            Configure(outputSettings);

            tuitionResolver.Initialize();

            var substitutions = @event.Substitutions.Select(substitution =>
            {
                /**
                 * Retrieve study groups by checking the subject (if any) and every given grade.
                 */ 
                var studyGroups = new List<string>();

                foreach (var grade in substitution.Grades)
                {
                    string tuition = null;
                    if (!string.IsNullOrEmpty(substitution.Subject))
                    {
                        tuition = tuitionResolver.ResolveStudyGroup(grade, substitution.Subject, substitution.Teachers.FirstOrDefault());
                    }
                    
                    if(tuition == null)
                    {
                        tuition = tuitionResolver.ResolveStudyGroup(grade);
                    }

                    if (tuition != null)
                    {
                        studyGroups.Add(tuition);
                    }
                    else
                    {
                        logger.LogDebug($"Did not find tuition for grade '{grade}' and subject '{substitution.Subject}'.");
                    }
                }

                // Remove duplicates
                studyGroups = studyGroups.Distinct().ToList();

                if (substitution.Grades.Count > 0 && studyGroups.Count == 0)
                {
                    logger.LogError($"Cannot resolve any tuition with subject '{substitution.Subject}' for grade [{string.Join(",", substitution.Grades)}]. Skip substitution.");
                    return null;
                }

                /**
                 * Resolve replacement grades only by their name (because the new subject must not be a valid tuition)
                 */
                var replacementStudyGroups = new List<string>();

                foreach (var grade in substitution.ReplacementGrades)
                {
                    string tuition = null;
                    if (!string.IsNullOrEmpty(substitution.ReplacementSubject))
                    {
                        tuition = tuitionResolver.ResolveStudyGroup(grade, substitution.ReplacementSubject, substitution.Teachers.FirstOrDefault());
                    }

                    if (tuition == null)
                    {
                        tuition = tuitionResolver.ResolveStudyGroup(grade);
                    }

                    if (tuition != null)
                    {
                        replacementStudyGroups.Add(tuition);
                    }
                }

                return new SubstitutionData
                {
                    Id = substitution.Id.ToString(),
                    Date = substitution.Date,
                    LessonStart = substitution.LessonStart + (substitution.IsSupervision ? 1 : 0), // advance start lesson by 1 if it is a supervision
                    LessonEnd = substitution.LessonEnd,
                    Type = substitution.Type,
                    Teachers = substitution.Teachers.ToList(),
                    ReplacementTeachers = substitution.ReplacementTeachers.ToList(),
                    Room = substitution.Room,
                    ReplacementRoom = substitution.ReplacementRoom,
                    Remark = substitution.Remark,
                    StartsBefore = substitution.IsSupervision,
                    StudyGroups = studyGroups,
                    ReplacementStudyGroups = replacementStudyGroups,
                    Subject = substitution.Subject,
                    ReplacementSubject = substitution.ReplacementSubject
                };
            }).Where(x => x != null);

            try
            {
                var result = substitutions.ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to compute substitutions.");
            }

            var response = await iccImporter.ImportSubstitutionsAsync(substitutions.ToList());
            await HandleResponseAsync(response);
        }

        protected override async Task HandleSupervisionEvent(SupervisionEvent @event, IIccOutput outputSettings)
        {
            Configure(outputSettings);

            var mapping = outputSettings.WeekMapping;
            var weeks = mapping.Weeks;

            if(mapping.UseWeekModulo)
            {
                weeks = weekMappingHelper.ComputeMapping(mapping.Weeks);
            }

            var supervisions = new List<TimetableSupervisionData>();

            foreach (var supervision in @event.Supervisions)
            {
                var supervisionWeeks = supervision.Weeks ?? Enumerable.Range(1, outputSettings.WeekMapping.Weeks.Count);

                if (string.IsNullOrEmpty(supervision.Teacher))
                {
                    logger.LogDebug("Skipping supervision with empty teacher.");
                    continue;
                }

                var namedWeeks = supervisionWeeks
                    .Select(x => weeks.ContainsKey(x) ? weeks[x] : null)
                    .Where(x => x != null)
                    .Distinct();

                foreach (var week in namedWeeks)
                {
                    supervisions.Add(new TimetableSupervisionData
                    {
                        Id = Guid.NewGuid().ToString(),
                        Lesson = supervision.Lesson,
                        Day = supervision.WeekDay,
                        Location = supervision.Location,
                        Teacher = supervision.Teacher,
                        IsBefore = true,
                        Week = week
                    });
                }
            }

            var response = await iccImporter.ImportSupervisionsAsync(outputSettings.SupervisionPeriod, supervisions.ToList());
            await HandleResponseAsync(response);
        }

        protected override async Task HandleTimetableEvent(TimetableEvent @event, IIccOutput outputSettings)
        {
            Configure(outputSettings);
            tuitionResolver.Initialize();

            if (outputSettings.TimetablePeriodMapping == null)
            {
                logger.LogError("TimetablePeriodMapping is null. Do not upload timetable.");
                return;
            }

            var period = outputSettings.TimetablePeriodMapping.ContainsKey(@event.Period) ? outputSettings.TimetablePeriodMapping[@event.Period] : null;

            if (period == null)
            {
                logger.LogDebug($"Cannot resolve period {@event.Period}. Skip upload.");
                return;
            }

            var lessons = new Dictionary<string, TimetableLessonData>();

            string computeAddedLessonKey(int day, int lesson, string week, string tuition, string room) => $"{day}-{lesson}-{week}-{tuition}-{room}";

            try
            {
                foreach (var lesson in @event.Lessons)
                { 
                    var tuition = tuitionResolver.ResolveTuition(lesson.Grade, lesson.Subject, lesson.Teacher);

                    if (tuition == null)
                    {
                        logger.LogDebug($"Cannot resolve tuition with subject {lesson.Subject} for grade {lesson.Grade} and teacher {lesson.Teacher}. Make freestyle lesson.");
                    }

                    foreach (var week in lesson.Weeks)
                    {
                        // ICC only supports double lessons max -> split lessons up!
                        var duration = lesson.LessonEnd - lesson.LessonStart;

                        if (duration > 1)
                        {
                            // Split lessons into separate lessons
                            for (int i = 0; i <= duration; i++)
                            {
                                var data = new TimetableLessonData
                                {
                                    Tuition = tuition,
                                    Lesson = lesson.LessonStart + i,
                                    IsDoubleLesson = false,
                                    Room = lesson.Room,
                                    Day = lesson.Day,
                                    Week = week
                                };

                                if (!string.IsNullOrEmpty(lesson.Teacher))
                                {
                                    data.Teachers.Add(lesson.Teacher);
                                }

                                if (tuition == null)
                                {
                                    data.Subject = lesson.Subject;
                                }

                                var id = computeAddedLessonKey(lesson.Day, lesson.LessonStart + i, week, tuition ?? lesson.Subject, lesson.Room);

                                if (!lessons.ContainsKey(id))
                                {
                                    lessons.Add(id, data);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(lesson.Teacher) && lessons[id].Teachers.Contains(lesson.Teacher))
                                    {
                                        lessons[id].Teachers.Add(lesson.Teacher);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var data = new TimetableLessonData
                            {
                                Tuition = tuition,
                                Lesson = lesson.LessonStart,
                                IsDoubleLesson = lesson.LessonStart != lesson.LessonEnd,
                                Room = lesson.Room,
                                Day = lesson.Day,
                                Week = week
                            };

                            if (!string.IsNullOrEmpty(lesson.Teacher))
                            {
                                data.Teachers.Add(lesson.Teacher);
                            }

                            if (tuition == null)
                            {
                                data.Subject = lesson.Subject;
                            }

                            var id = computeAddedLessonKey(lesson.Day, lesson.LessonStart, week, tuition ?? lesson.Subject, lesson.Room);
                            var nextLessonId = computeAddedLessonKey(lesson.Day, lesson.LessonStart + 1, week, tuition ?? lesson.Subject, lesson.Room);

                            if (lessons.ContainsKey(id))
                            {
                                foreach (var teacher in lessons[id].Teachers)
                                {
                                    if (!data.Teachers.Contains(teacher))
                                    {
                                        data.Teachers.Add(teacher);
                                    }
                                }
                            }

                            if (lessons.ContainsKey(nextLessonId))
                            {
                                foreach (var teacher in lessons[nextLessonId].Teachers)
                                {
                                    if (!data.Teachers.Contains(teacher))
                                    {
                                        data.Teachers.Add(teacher);
                                    }
                                }
                            }

                            if (data.IsDoubleLesson)
                            {
                                if (lessons.ContainsKey(id))
                                {
                                    lessons[id] = data;
                                }
                                else
                                {
                                    lessons.Add(id, data);
                                }

                                if (lessons.ContainsKey(nextLessonId))
                                {
                                    lessons[nextLessonId] = data;
                                }
                                else
                                {
                                    lessons.Add(nextLessonId, data);
                                }
                            }
                            else if (!lessons.ContainsKey(id))
                            {
                                lessons.Add(id, data);
                            }
                            else if (lessons.ContainsKey(id))
                            {
                                lessons[id] = data;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Something went wrong while constructing all timetable lessons.");
                return;
            }

            var response = await iccImporter.ImportTimetableLessonsAsync(period, lessons.Select(x => { x.Value.Id = x.Key; return x.Value; }).Distinct().ToList());
            await HandleResponseAsync(response);
        }

        protected override async Task HandleFreeLessonEvent(FreeLessonEvent @event, IIccOutput outputSettings)
        {
            Configure(outputSettings);

            var freeLessons = @event.FreeLessons.Select(freeLesson =>
            {
                return new FreeLessonTimespanData
                {
                    Start = freeLesson.Start,
                    End = freeLesson.End,
                    Date = freeLesson.Date
                };
            });

            var response = await iccImporter.ImportFreeLessonTimespansAsync(freeLessons.ToList());
            await HandleResponseAsync(response);
        }

        protected override Task HandleTuitionEvent(TuitionEvent @event, IIccOutput outputSettings)
        {
            throw new NotImplementedException();
        }

        private string GetStringOrNull(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return input;
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

        private string GetOrComputeId(Exam exam)
        {
            if(exam.Id.HasValue)
            {
                return exam.Id.Value.ToString();
            }

            var gradesAsString = string.Join('-', exam.Grades.OrderBy(x => x));
            var coursesAsString = string.Join('-', exam.Courses.OrderBy(x => x));

            return $"{exam.Date:yyyy-MM-dd}-{gradesAsString}-{coursesAsString}-{exam.Name}";
        }

    }
}
