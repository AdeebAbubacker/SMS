
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class ClassBatch
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public string? Semester { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Course? Course { get; set; }
        public User? Teacher { get; set; }

        public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
        public ICollection<Attendance> AttendanceRecords { get; set; } = new List<Attendance>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}