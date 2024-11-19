using System;

public class TaskViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }
    public Guid UserId { get; set; }  
}