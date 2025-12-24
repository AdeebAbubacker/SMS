using FluentValidation;
using SchoolManagementSystem.Application.DTOs.Department;

namespace SchoolManagementSystem.Application.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDTO>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed {MaxLength} characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.HeadOfDepartmentId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("{PropertyName} must be a valid GUID when provided.");
        }
    }
}
