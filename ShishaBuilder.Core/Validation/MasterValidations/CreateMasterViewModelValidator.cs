using FluentValidation;
using ShishaBuilder.Core.Dtos.MasterDtos;

namespace ShishaBuilder.Core.Validation.MasterValidations;

public class CreateMasterViewModelValidator :AbstractValidator<CreateMasterViewModel>
{

    int nameMaxLength=30;
    int surnameMaxLength=30;
    public CreateMasterViewModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(nameMaxLength).WithMessage("First name cannot exceed 30 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(surnameMaxLength).WithMessage("Last name cannot exceed 30 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{7,15}$").WithMessage("Phone number must be valid and contain 7 to 15 digits.");

    }
}
