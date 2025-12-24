
using SchoolManagementSystem.Domain.Models;

namespace SchoolManagementSystem.Application.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;

        public int Credits { get; set; }
        
        public List<ClassBatch> Classes { get; set; } = new();
    }
}