using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Management_System_API_1.Entity_Models;
using Task_Management_System_API_1.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Task_Management_System_API_1.Entity_Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }  // "Pending", "In Progress", "Completed"
        public string AssignedUserId { get; set; }  
        public ApplicationUser AssignedUser { get; set; }
    }
}
