namespace Task_Management_System_API_1.ViewModels
{
    public class TaskDetailViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }
    }
}
