using FluentValidation;
using TaskManager.Api.Application.Query;

namespace TaskManager.Api.Application.Validators;

/// <summary>
/// Valida el id antes de ir a la DB. Evita una ida innecesaria a Postgres
/// cuando el cliente manda algo inválido (ej. 0 o negativo).
/// </summary>
public class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive number.");
    }
}
