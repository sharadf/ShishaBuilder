using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ShishaBuilder.Core.Dtos;

namespace ShishaBuilder.Core.Validation;

public class EditTobaccoViewModelValidator :AbstractValidator<EditTobaccoViewModel>
{
    int nameMaxLength=30;
    int brandMaxLength=30;
    int flavorMaxLength=100;

    public EditTobaccoViewModelValidator ()
    {
        RuleFor((CreatedTobacco)=>CreatedTobacco.Name)
            .NotEmpty()
                .WithMessage("Name cannot be empty.")
            .MaximumLength(nameMaxLength)
                .WithMessage($"Name cannot be more than {nameMaxLength} characters.");

        RuleFor((CreatedTobacco)=>CreatedTobacco.Brand)
            .MaximumLength(brandMaxLength)
                .WithMessage($"Brand cannot be more than {brandMaxLength} characters.");
        
        RuleFor((CreatedTobacco)=>CreatedTobacco.Flavor)
            .MaximumLength(flavorMaxLength)
                .WithMessage($"Flavor cannot be more than {flavorMaxLength} characters.");

        RuleFor((CreatedTobacco)=>CreatedTobacco.Strength)
            .NotEmpty()
                .WithMessage("Strength must be selected.");
    }
}
