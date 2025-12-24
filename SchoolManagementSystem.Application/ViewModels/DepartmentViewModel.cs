
namespace SchoolManagementSystem.Application.ViewModels
{
    public class DepartmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? HeadOfDepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
