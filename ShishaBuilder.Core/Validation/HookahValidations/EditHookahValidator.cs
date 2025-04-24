using FluentValidation;
using ShishaBuilder.Core.DTOs.HookahDtos;

namespace ShishaBuilder.Core.Validation.HookahValidations;

public class EditookahValidator : AbstractValidator<EditHookah>
{
    private int modelNameLength = 30;
    private int companyNameLength = 30;

    public EditookahValidator()
    {
        RuleFor((CreatedHookah) => CreatedHookah.ModelName)
            .NotEmpty()
                .WithMessage("Model name cannot be empty.")
            .MaximumLength(modelNameLength)
                .WithMessage($"Name cannot be more than {modelNameLength} characters.");

        RuleFor((CreatedHookah) => CreatedHookah.CompanyName)
            .MaximumLength(companyNameLength)
                .WithMessage($"Company name cannot be more than {companyNameLength} characters.");
    }
}