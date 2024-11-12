using System.ComponentModel.DataAnnotations;

namespace Task_Management_System_API_1.ViewModels
{
    public class TaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } // Options: "Pending", "In Progress", "Completed"
    }
}
