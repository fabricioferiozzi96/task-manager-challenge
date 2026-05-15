using FluentValidation;
using MediatR;
using ValidationException = TaskManager.Api.Application.Exceptions.ValidationException;

namespace TaskManager.Api.Application.Behaviors;
/// Antes de que cualquier handler corra, este behavior:
/// 1) Busca todos los IValidator&lt;TRequest&gt; registrados en el DI.
/// 2) Los ejecuta contra el request.
/// 3) Si alguno falla, lanza una <see cref="ValidationException"/> y NUNCA
///    llega al handler.
///
/// Resultado: los handlers solo se ocupan del caso de uso, no de defenderse
/// del input. La validación es transversal a todas las queries y commands
/// — un solo lugar para mantenerla.
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());

        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await next();
    }
}
