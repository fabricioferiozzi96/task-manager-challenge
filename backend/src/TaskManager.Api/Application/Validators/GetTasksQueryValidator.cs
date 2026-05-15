using FluentValidation;
using TaskManager.Api.Application.Query;

namespace TaskManager.Api.Application.Validators;

/// <summary>
/// FluentValidation reemplaza al "if (status &lt; 1 || status &gt; 4) BadRequest"
/// que estaba metido en el controller. Vive en Application — es la regla del
/// caso de uso, no del transporte HTTP.
///
/// Ventajas vs validar en el controller:
/// - Una sola fuente de verdad: si la regla cambia, se actualiza en un solo lugar.
/// - Composición: se pueden encadenar reglas (.GreaterThan().LessThan().WithMessage()).
/// - Mensajes consistentes y traducibles.
/// - Se ejecuta automáticamente por el ValidationBehavior, sin que el handler
///   ni el controller tengan que llamarla a mano.
/// </summary>
public class GetTasksQueryValidator : AbstractValidator<GetTasksQuery>
{
    public GetTasksQueryValidator()
    {
        RuleFor(x => x.StatusId)
            .InclusiveBetween(1, 4)
            .When(x => x.StatusId.HasValue)
            .WithMessage("Invalid status. Must be between 1 and 4.");

        RuleFor(x => x.PriorityId)
            .InclusiveBetween(1, 4)
            .When(x => x.PriorityId.HasValue)
            .WithMessage("Invalid priority. Must be between 1 and 4.");
    }
}
