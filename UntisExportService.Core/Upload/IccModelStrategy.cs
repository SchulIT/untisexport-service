using SchulIT.UntisExport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UntisExportService.Core.Model;
using UntisExportService.Core.Settings;
using UntisExportService.Core.StudyGroups;

namespace UntisExportService.Core.Upload
{
    public class IccModelStrategy : IModelStrategy
    {

        private readonly IStudyGroupResolver studyGroupResolver;

        public IccModelStrategy(IStudyGroupResolver studyGroupResolver)
        {
            this.studyGroupResolver = studyGroupResolver;
        }

        public IEnumerable<IInfotext> GetInfotexts(IEnumerable<Infotext> infotexts)
        {
            return infotexts.Select(x => new IccInfotext { Date = x.Date, Content = x.Text }).Cast<IInfotext>();
        }

        public IEnumerable<ISubstitution> GetSubstitutions(IEnumerable<Substitution> substitutions)
        {
            studyGroupResolver.Initialize();

            var result = new List<ISubstitution>();

            foreach(var x in substitutions)
            {
                var substitution = new IccSubstitution
                {
                    Id = x.Id,
                    Date = x.Date,
                    LessonStart = x.LessonStart,
                    LessonEnd = x.LessonEnd,
                    Teachers = x.Teachers,
                    ReplacementTeachers = x.ReplacementTeachers,
                    Subject = x.Subject,
                    ReplacementSubject = x.ReplacementSubject,
                    Room = x.Room,
                    ReplacementRoom = x.ReplacementRoom,
                    Type = x.Type,
                    Remark = x.Remark,
                    IsSupervision = x.IsSupervision
                };

                // Study Groups
                substitution.StudyGroups = x.Grade.Select(g => studyGroupResolver.Resolve(g, substitution.Subject)).Where(sg => !string.IsNullOrEmpty(sg)).Distinct().ToList();
                substitution.ReplacementStudyGroups = x.ReplacementGrades.Select(g => studyGroupResolver.Resolve(g, substitution.ReplacementSubject)).Where(sg => !string.IsNullOrEmpty(sg)).Distinct().ToList();

                result.Add(substitution);
            }

            return result;
        }

        public bool IsSupported(ISettings settings)
        {
            return settings.Endpoint.UseLegacyStrategy == false;
        }

        public IEnumerable<IAbsence> GetAbsences(IEnumerable<Absence> absences)
        {
            return absences.Select(x => new IccAbsence { Date = x.Date, Objective = x.Objective, LessonEnd = x.LessonEnd, LessonStart = x.LessonStart, Type = ConvertAbsenceType(x.Type) }).Cast<IAbsence>();
        }

        private IccAbsence.IccAbsenceType ConvertAbsenceType(Absence.ObjectiveType type)
        {
            switch (type)
            {
                case Absence.ObjectiveType.StudyGroup:
                    return IccAbsence.IccAbsenceType.StudyGroup;

                case Absence.ObjectiveType.Teacher:
                    return IccAbsence.IccAbsenceType.Teacher;
            }

            throw new ArgumentException($"Invalid ObjectiveType { type } provided.");
        }
    }
}
