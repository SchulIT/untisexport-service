using Microsoft.Extensions.Logging;
using SchulIT.IccImport;
using SchulIT.IccImport.Models;
using SchulIT.IccImport.Response;
using SchulIT.UntisExport.Substitutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private readonly WeekMappingHelper weekMappingHelper;
        private readonly ITuitionResolver tuitionResolver;
        private readonly IIccImporter iccImporter;
        private readonly ILogger<IccOutputHandler> logger;

        public IccOutputHandler(ITuitionResolver tuitionResolver, IIccImporter iccImporter, WeekMappingHelper weekMappingHelper, ILogger<IccOutputHandler> logger)
        {
            this.weekMappingHelper = weekMappingHelper;
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
                logger.LogError($"Upload was not successful: {(response as ErrorResponse).Message}.");
            }
            else if (response is ImportResponse)
            {
                var importResponse = response as ImportResponse;
                logger.LogInformation($"Import successful: {importResponse.AddedCount} items added, {importResponse.UpdatedCount} items updated and {importResponse.RemovedCount} items removed.");
            }
            else if (response is SuccessReponse)
            {
                logger.LogInformation("Upload successful.");
            }

            // TODO: Store response in file

            return Task.CompletedTask;
        }

        private string GetOrComputeId(int? id)
        {
            if(id != null)
            {
                return id.ToString();
            }

            return Guid.NewGuid().ToString();
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

                return new ExamData
                {
                    Id = GetOrComputeId(exam.Id), // TODO
                    Date = exam.Date,
                    LessonStart = exam.LessonStart,
                    LessonEnd = exam.LessonEnd,
                    Description = exam.Remark,
                    Rooms = exam.Rooms.ToList(),
                    Supervisions = exam.Supervisions.ToList(),
                    Tuitions = tuitions
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
                    if (string.IsNullOrEmpty(substitution.Subject))
                    {
                        tuition = tuitionResolver.ResolveStudyGroup(grade);
                    }
                    else
                    {
                        tuition = tuitionResolver.ResolveStudyGroup(grade, substitution.Subject, substitution.Teachers.FirstOrDefault());
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
                    var tuition = tuitionResolver.ResolveStudyGroup(grade);

                    if (tuition != null)
                    {
                        replacementStudyGroups.Add(tuition);
                    }
                    else
                    {
                        logger.LogDebug($"Did not find study group for grade '{grade}'.");
                    }
                }

                return new SubstitutionData
                {
                    Id = GetOrComputeId(substitution.Id),
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
                        Id = GetOrComputeId(null),
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

            if(outputSettings.TimetablePeriodMapping == null)
            {
                logger.LogError("TimetablePeriodMapping is null. Do not upload timetable.");
                return;
            }


            var period = outputSettings.TimetablePeriodMapping.ContainsKey(@event.Period) ? outputSettings.TimetablePeriodMapping[@event.Period] : null;

            if (period == null)
            {
                logger.LogError($"Cannot resolve period {@event.Period}. Skip upload.");
                return;
            }

            var lessons = new List<TimetableLessonData>();
            var addedLessons = new List<string>();

            string computeAddedLessonKey(int day, int lesson, string week, string tuition) => $"{day}-{lesson}-{week}-{tuition}";

            foreach (var lesson in @event.Lessons)
            {
                var tuition = tuitionResolver.ResolveTuition(lesson.Grade, lesson.Subject, lesson.Teacher);

                if(tuition == null)
                {
                    logger.LogError($"Cannot resolve tuition with subject {lesson.Subject} for grade {lesson.Grade} and teacher {lesson.Teacher}. Skip lesson.");
                    continue;
                }

                foreach (var week in lesson.Weeks)
                {
                    // ICC only supports double lessons max -> split lessons up!
                    var duration = lesson.LessonEnd - lesson.LessonStart;

                    if (duration > 1)
                    {
                        // Split lessons into separate lessons
                        for(int i = 0; i <= duration; i++)
                        {
                            var data = new TimetableLessonData
                            {
                                Id = GetOrComputeId(null),
                                Tuition = tuition,
                                Lesson = lesson.LessonStart + i,
                                IsDoubleLesson = false,
                                Room = lesson.Room,
                                Day = lesson.Day,
                                Week = week
                            };

                            var id = computeAddedLessonKey(lesson.Day, lesson.LessonStart + i, week, tuition);

                            if(!addedLessons.Contains(id))
                            {
                                lessons.Add(data);
                                addedLessons.Add(id);
                            }
                        }
                    }
                    else
                    {
                        var data = new TimetableLessonData
                        {
                            Id = GetOrComputeId(null),
                            Tuition = tuition,
                            Lesson = lesson.LessonStart,
                            IsDoubleLesson = lesson.LessonStart != lesson.LessonEnd,
                            Room = lesson.Room,
                            Day = lesson.Day,
                            Week = week
                        };

                        var id = computeAddedLessonKey(lesson.Day, lesson.LessonStart, week, tuition);

                        if (!addedLessons.Contains(id))
                        {
                            lessons.Add(data);
                            addedLessons.Add(id);
                        }
                    }
                }
            }

            var response = await iccImporter.ImportTimetableLessonsAsync(period, lessons);
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
    }
}
