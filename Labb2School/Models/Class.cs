using System;
using System.Collections.Generic;

namespace Labb2School.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int ClassTeacherId { get; set; }

    public int GradeLevel { get; set; }

    public string SchoolYear { get; set; } = null!;

    public virtual Staff ClassTeacher { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
