namespace Task_Management_System_API_1.Entity_Models
{
    public class DeleteClaimRequest
    {
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
