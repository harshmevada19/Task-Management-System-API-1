namespace Task_Management_System_API_1.Entity_Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid TaskId { get; set; }
    }
}
