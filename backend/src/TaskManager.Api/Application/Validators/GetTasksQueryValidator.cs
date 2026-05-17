using FluentValidation;
using TaskManager.Api.Application.Query;

namespace TaskManager.Api.Application.Validators;
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
