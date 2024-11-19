namespace Task_Management_System_API_1.Entity_Models
{
    public class Role
    {
        public int RoleId { get; set; }      
        public string RoleName { get; set; }  // Example: "Admin", "User"
        public bool IsActive { get; set; }    
    }
}
