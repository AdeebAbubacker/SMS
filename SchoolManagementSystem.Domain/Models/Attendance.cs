using SchoolManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Models
{
    public class Attendance
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClassBatchId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }
        public Guid MarkedByTeacherId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ClassBatch? ClassBatch { get; set; }
        public User? Student { get; set; }
    }
}