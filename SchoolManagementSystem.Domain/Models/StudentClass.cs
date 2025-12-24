using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class StudentClass
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StudentId { get; set; }
        public Guid ClassBatchId { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        public User? Student { get; set; }
        public ClassBatch? ClassBatch { get; set; }
    }
}