using System.ComponentModel.DataAnnotations;

namespace Task_Management_System_API_1.Entity_Models
{
    public class UserRole
    {
        [Key]
        public string Username { get; set; } 
        public string Role { get; set; } 
    }
}
