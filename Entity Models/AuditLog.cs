namespace Task_Management_System_API_1.Entity_Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }
        public DateTime ActionTime { get; set; }
    }
}
