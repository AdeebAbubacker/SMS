using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; } = null!;
        public Guid UserId { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Revoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked == null && !IsExpired;

        public User User { get; set; } = null!;
    }
}