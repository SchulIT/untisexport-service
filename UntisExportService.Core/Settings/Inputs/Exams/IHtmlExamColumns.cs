namespace UntisExportService.Core.Settings.Inputs.Exams
{
    public interface IHtmlExamColumns
    {
        string CoursesColumn { get; set; }
        char CoursesSeparator { get; set; }
        string DateColumn { get; set; }
        string GradesColumn { get; set; }
        char GradesSeparator { get; set; }
        string LessonEndColumn { get; set; }
        string LessonStartColumn { get; set; }
        string NameColumn { get; set; }
        string RemarkColumn { get; set; }
        string RoomsColumn { get; set; }
        char RoomsSeparator { get; set; }
        string TeachersColumn { get; set; }
        char TeachersSeparator { get; set; }
    }
}