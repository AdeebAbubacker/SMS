using FluentValidation;
using SchoolManagementSystem.Application.DTOs.Course;

namespace SchoolManagementSystem.Application.Validators
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");

            RuleFor(x => x.Code)
               .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Credits)
              .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed {MaxLength} characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("{PropertyName} is required and must be a valid GUID.");
        }
    }
}
