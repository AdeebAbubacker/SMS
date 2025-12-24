
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public Guid DepartmentId { get; set; }
        public int Credits { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual Department? Department { get; set; }
        public ICollection<ClassBatch> Classes { get; set; } = new List<ClassBatch>();
    }
}