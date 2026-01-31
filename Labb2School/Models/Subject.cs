using System;
using System.Collections.Generic;

namespace Labb2School.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public string CourseCode { get; set; } = null!;

    public int GradeLevel { get; set; }

    public string? Language { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
