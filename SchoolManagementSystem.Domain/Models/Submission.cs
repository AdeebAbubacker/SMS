
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Submission
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AssignmentId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
        public string? FileUrl { get; set; }
        public decimal? Grade { get; set; }
        public Guid? GradedByTeacherId { get; set; }
        public string? Remarks { get; set; }

        public Assignment Assignment { get; set; } = null!;
        public User Student { get; set; } = null!;
    }
}