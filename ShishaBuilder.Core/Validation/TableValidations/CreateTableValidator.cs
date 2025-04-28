using FluentValidation;
using ShishaBuilder.Core.DTOs.HookahDtos;
using ShishaBuilder.Core.DTOs.TableDtos;

namespace ShishaBuilder.Core.Validation.HookahValidations;

public class CreateTableValidator : AbstractValidator<CreateTable>
{
    private int maximumTableCount = 50;
    private int minimumTableCount=1;
    public CreateTableValidator()
    {
        RuleFor((CreatedTable) => CreatedTable.TableNumber)
            .NotEmpty()
                .WithMessage("Table must have a value.")
            .LessThan(maximumTableCount)
                .WithMessage($"Table quantity can not be more than {maximumTableCount} tables.")
            .GreaterThanOrEqualTo(minimumTableCount)
                .WithMessage($"Table quantity can not be less than {minimumTableCount} tables.");

    }
}
