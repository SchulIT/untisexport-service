﻿namespace UntisExportService.Core.Settings.Inputs.Substitutions
{
    public interface IHtmlSubstitutionColumns
    {
        string IdColumn { get; }
        string DateColumn { get; }
        string LessonColumn { get; }
        string GradesColumn { get; }
        string ReplacementGradesColumn { get; }
        string TeachersColumn { get; }
        string ReplacementTeachersColumn { get; }
        string SubjectColumn { get; }
        string ReplacementSubjectColumn { get; }
        string RoomColumn { get; }
        string ReplacementRoomColumn { get; }
        string TypeColumn { get; }
        string RemarkColumn { get; }
    }
}
