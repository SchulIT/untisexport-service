using SchulIT.UntisExport.Exams.Html;
using SchulIT.UntisExport.Substitutions.Html;
using SchulIT.UntisExport.Timetable.Html;
using System.Linq;
using UntisExportService.Core.Settings.Inputs.Exams;
using UntisExportService.Core.Settings.Inputs.Substitutions;
using UntisExportService.Core.Settings.Inputs.Timetable;

namespace UntisExportService.Core.Extensions
{
    public static class SettingsEx
    {
        public static ExamColumnSettings ToUntis(this IHtmlExamColumns settings)
        {
            return new ExamColumnSettings
            {
                DateColumn = settings.DateColumn,
                LessonStartColumn = settings.LessonStartColumn,
                LessonEndColumn = settings.LessonEndColumn,
                GradesColumn = settings.GradesColumn,
                GradesSeparator = settings.GradesSeparator,
                CoursesColumn = settings.CoursesColumn,
                CoursesSeparator = settings.CoursesSeparator,
                TeachersColumn = settings.TeachersColumn,
                TeachersSeparator = settings.TeachersSeparator,
                RoomsColumn = settings.RoomsColumn,
                RoomsSeparator = settings.RoomsSeparator,
                NameColumn = settings.NameColumn,
                RemarkColumn = settings.RemarkColumn
            };
        }

        public static SubstitutionExportSettings ToUntis(this IHtmlSubstitutionInput input)
        {
            return new SubstitutionExportSettings
            {
                DateTimeFormat = input.Options.DateTimeFormat,
                FixBrokenPTags = input.Options.FixBrokenPTags,
                EmptyValues = input.Options.EmptyValues.ToList(),
                IncludeAbsentValues = input.Options.InlcudeAbsentValues,
                ColumnSettings = input.ColumnSettings.ToUntis(),
                AbsenceSettings = input.AbsenceSettings.ToUntis()
            };
        }

        public static AbsenceSettings ToUntis(this IHtmlAbsenceSettings settings)
        {
            return new AbsenceSettings
            {
                ParseAbsences = settings.ParseAbsences,
                TeacherIdentifier = settings.TeacherIdentifier,
                StudyGroupIdentifier = settings.StudyGroupIdentifier
            };
        }

        public static SubstitutionColumnSettings ToUntis(this IHtmlSubstitutionColumns settings)
        {
            return new SubstitutionColumnSettings
            {
                IdColumn = settings.IdColumn,
                DateColumn = settings.DateColumn,
                LessonColumn = settings.LessonColumn,
                GradesColumn = settings.GradesColumn,
                ReplacementGradesColumn = settings.ReplacementGradesColumn,
                TeachersColumn = settings.TeachersColumn,
                ReplacementTeachersColumn = settings.ReplacementTeachersColumn,
                SubjectColumn = settings.SubjectColumn,
                ReplacementSubjectColumn = settings.ReplacementSubjectColumn,
                RoomColumn = settings.RoomColumn,
                ReplacementRoomColumn = settings.ReplacementRoomColumn,
                TypeColumn = settings.TypeColumn,
                RemarkColumn = settings.RemarkColumn
            };
        }

        public static TimetableExportSettings ToUntis(this ITimetableInput settings)
        {
            return new TimetableExportSettings
            {
                FirstLesson = settings.FirstLesson,
                UseWeeks = settings.UseWeeks
            };
        }
    }
}
