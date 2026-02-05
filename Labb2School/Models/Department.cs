using System;
using System.Collections.Generic;

namespace Labb2School.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
