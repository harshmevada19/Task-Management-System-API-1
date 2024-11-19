using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

public class User 
{
    public Guid UserId { get; set; }
    public string name { get; set; }
    public bool IsActive { get; set; } = true;
    public int Role { get; set; } 

}
