

namespace SchoolManagementSystem.Application.DTOs.Department
{
    public class CreateDepartmentDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? HeadOfDepartmentId { get; set; }
    }
}
