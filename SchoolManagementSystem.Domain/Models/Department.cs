
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Domain.Models
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? HeadOfDepartmentId { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}