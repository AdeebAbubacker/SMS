
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Assignment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClassBatchId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid MarkedByTeacherId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ClassBatch ClassBatch { get; set; } = null!;
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}