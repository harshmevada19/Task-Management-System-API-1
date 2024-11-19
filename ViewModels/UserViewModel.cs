using System;
using System.Collections.Generic;

public class UserViewModel
{
    public string name { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Task> Tasks { get; set; }
}
