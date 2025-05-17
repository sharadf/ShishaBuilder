using FluentValidation;
using ShishaBuilder.Core.DTOs.LoginDtos;

namespace ShishaBuilder.Core.Validation.MasterValidations;

public class MasterRegistrationDtoValidator : AbstractValidator<MasterRegistrationDto>
{
    public MasterRegistrationDtoValidator()
    {
        RuleFor(x => x.Login).NotEmpty().WithMessage("Login is required.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MaximumLength(60)
            .WithMessage("Full name cannot exceed 60 characters.");

        RuleFor(x => x.Age).InclusiveBetween(18, 65).WithMessage("Age must be between 18 and 65");

        RuleFor(x => x.ExperienceYears)
            .InclusiveBetween(2, 40)
            .WithMessage("Experience must be between 2 and 40 years");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("Gender is required.")
            .Must(g => g == "Male" || g == "Female")
            .WithMessage("Gender must be either 'Male' or 'Female'");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{7,15}$")
            .WithMessage("Phone number must be valid and contain 7 to 15 digits.");

        When(
            x => x.ImageFile != null,
            () =>
            {
                RuleFor(x => x.ImageFile!.Length)
                    .LessThanOrEqualTo(5 * 1024 * 1024) // 5 MB
                    .WithMessage("Image file size cannot exceed 5 MB.");

                RuleFor(x => x.ImageFile!.ContentType)
                    .Must(ct => ct == "image/jpeg" || ct == "image/png" || ct == "image/jpg" )
                    .WithMessage("Only JPEG and PNG formats are allowed.");
            }
        );
    }
}
