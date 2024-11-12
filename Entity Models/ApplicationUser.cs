using Microsoft.AspNetCore.Identity;

namespace Task_Management_System_API_1.Entity_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
