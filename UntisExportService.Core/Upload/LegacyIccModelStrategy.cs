using SchulIT.UntisExport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UntisExportService.Core.Model;
using UntisExportService.Core.Settings;

namespace UntisExportService.Core.Upload
{
    /// <summary>
    /// For the current ICC we need substitutions to be split up for each lesson. Also, we only support
    /// one teacher/replacement teacher. Thus, we put additional (replacement) teachers into the description
    /// of the substitution.
    /// </summary>
    public class LegacyIccModelStrategy : IModelStrategy
    {
        public IEnumerable<IInfotext> GetInfotexts(IEnumerable<Infotext> infotexts)
        {
            return infotexts.Select(x => new IccInfotext { Date = x.Date, Content = x.Text }).Cast<IInfotext>();
        }

        public IEnumerable<ISubstitution> GetSubstitutions(IEnumerable<Substitution> substitutions)
        {
            var result = new List<ISubstitution>();

            foreach (var x in substitutions)
            {
                for (int lesson = x.LessonStart; lesson <= x.LessonEnd; lesson++)
                {
                    var substitution = new LegacyIccSubstitution
                    {
                        Id = x.Id,
                        Date = x.Date,
                        Lesson = lesson,
                        AbsenceTeacher = x.Teachers.FirstOrDefault(),
                        ReplacementTeacher = x.ReplacementTeachers.FirstOrDefault(),
                        Subject = x.Subject,
                        ReplacementSubject = x.ReplacementSubject,
                        Room = x.Room,
                        ReplacementRoom = x.ReplacementRoom,
                        Grades = x.Grade.ToArray(),
                        ReplacementGrades = x.ReplacementGrades.ToArray(),
                        LastChange = DateTime.Now,
                        ReplacementType = x.Type,
                        Type = 0
                    };

                    var description = x.Remark;
                    var additionalTeachers = x.Teachers.Skip(1).ToArray();
                    var additionalReplacementTeachers = x.ReplacementTeachers.Skip(1).ToArray();
                    var additionalTeachersAsString = string.Join(", ", additionalTeachers);
                    var additionalReplacementTeachersAsString = string.Join(", ", additionalReplacementTeachers);


                    if (additionalTeachers.Length > 0 && additionalReplacementTeachers.Length > 0)
                    {
                        description += $" ({additionalTeachersAsString} → {additionalReplacementTeachersAsString})";
                    }
                    else if (additionalTeachers.Length > 0)
                    {
                        description += $" ({additionalTeachersAsString})";
                    }
                    else if (additionalReplacementTeachers.Length > 0)
                    {
                        description += $" ({additionalReplacementTeachersAsString})";
                    }

                    substitution.Description = description;
                    result.Add(substitution);
                }
            }

            return result;
        }

        public bool IsSupported(ISettings settings)
        {
            return settings.Endpoint.UseNewVersion == false;
        }
    }
}
