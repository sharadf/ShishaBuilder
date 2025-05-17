using FluentValidation;
using ShishaBuilder.Core.DTOs.LoginDtos;

public class MasterRegistrationValidator : AbstractValidator<MasterRegistrationDto>
{
    public MasterRegistrationValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Age).InclusiveBetween(18, 65);
        RuleFor(x => x.ExperienceYears).InclusiveBetween(2, 40);
        RuleFor(x => x.Gender).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?\d{10,15}$");
    }
}
