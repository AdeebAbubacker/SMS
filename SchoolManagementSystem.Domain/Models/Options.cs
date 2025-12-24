namespace SchoolManagementSystem.Domain.Models
{
    public class JWTOptions
    {
        public required string Secret { get; set; }
        public required string ValidAudience { get; set; }
        public required string ValidIssuer { get; set; }
    }
}