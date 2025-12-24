using FluentValidation;
using SchoolManagementSystem.Application.DTOs.Auth;
using SchoolManagementSystem.Domain.Enums;

public class RegisterRequestDTOValidator : AbstractValidator<RegisterRequestDTO>
{
    public RegisterRequestDTOValidator()
    {
        // NAME
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        // EMAIL
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        // PASSWORD STRENGTH
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one symbol.");

        // ROLE VALIDATION
        RuleFor(x => x.Role)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Role is required.")
            .Must(BeAValidRole)
            .WithMessage("Role must be one of: Admin, Teacher, Student.");
    }

    private bool BeAValidRole(string role)
    {
        return Enum.TryParse(typeof(Role), role, true, out _);
    }
}
