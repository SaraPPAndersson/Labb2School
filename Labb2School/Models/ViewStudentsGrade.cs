using System;
using System.Collections.Generic;

namespace Labb2School.Models;

public partial class ViewStudentsGrade
{
    public string StudentName { get; set; } = null!;

    public string? Subject { get; set; }

    public string? CourseCode { get; set; }

    public string? Grade { get; set; }

    public string? ByTeacher { get; set; }

    public DateOnly? SetOn { get; set; }
}
