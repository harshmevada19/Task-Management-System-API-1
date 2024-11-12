using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management_System_API_1.Entity_Models
{
    public class UserTask
    {
        public int UserId { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}
