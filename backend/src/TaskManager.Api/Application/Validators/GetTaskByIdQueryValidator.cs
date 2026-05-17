using FluentValidation;
using TaskManager.Api.Application.Query;

namespace TaskManager.Api.Application.Validators;
public class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery>
{
    public GetTaskByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be a positive number.");
    }
}
