using System;
using System.Collections.Generic;

namespace Labb2School.Models;

public partial class GetStudentsGrade
{
    public string StudentName { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public string Grade { get; set; } = null!;

    public string ByTeacher { get; set; } = null!;

    public DateOnly SetOn { get; set; }
}
