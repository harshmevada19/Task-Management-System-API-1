﻿namespace Task_Management_System_API_1.Entity_Models
{
    public class UpdateClaimRequest
    {
        public string UserId { get; set; }
        public string OldClaimType { get; set; }
        public string OldClaimValue { get; set; }
        public string NewClaimType { get; set; }
        public string NewClaimValue { get; set; }
    }
}