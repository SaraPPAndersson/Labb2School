using System;
using System.Collections.Generic;

namespace Labb2School.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int ClassId { get; set; }

    public string PersonalNumber { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
