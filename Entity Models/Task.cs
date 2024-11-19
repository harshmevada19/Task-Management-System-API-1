using System;
using System.ComponentModel.DataAnnotations;

public class Task
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }  // Navigation property
}
