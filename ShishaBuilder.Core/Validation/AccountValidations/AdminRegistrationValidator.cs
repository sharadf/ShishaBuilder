using FluentValidation;

public class AdminRegistrationValidator : AbstractValidator<AdminRegistrationDto>
{
    public AdminRegistrationValidator()
    {
        RuleFor(x => x.Login).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?\d{10,15}$");
    }
}