using FluentAssertions;
using FluentValidation.TestHelper;
using TaskManager.Api.Application.Query;
using TaskManager.Api.Application.Validators;

namespace TaskManager.UnitTests.Validators;

public class GetTasksQueryValidatorTests
{
    private readonly GetTasksQueryValidator _validator = new();

    [Theory]
    [InlineData(null, null)]
    [InlineData(1, null)]
    [InlineData(null, 4)]
    [InlineData(2, 3)]
    public void Accepts_valid_or_missing_filters(int? status, int? priority)
    {
        var result = _validator.TestValidate(new GetTasksQuery(status, priority));
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(-1)]
    public void Rejects_status_outside_1_to_4(int invalid)
    {
        var result = _validator.TestValidate(new GetTasksQuery(invalid, null));
        result.ShouldHaveValidationErrorFor(x => x.StatusId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void Rejects_priority_outside_1_to_4(int invalid)
    {
        var result = _validator.TestValidate(new GetTasksQuery(null, invalid));
        result.ShouldHaveValidationErrorFor(x => x.PriorityId);
    }
}
