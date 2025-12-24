namespace SchoolManagementSystem.Application.DTOs.Course
{
    public class CreateCourseDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }

        public Guid DepartmentId { get; set; }

        public int Credits { get; set; }
    }
}