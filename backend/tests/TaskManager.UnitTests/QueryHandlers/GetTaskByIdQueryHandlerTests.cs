using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TaskManager.Api.Application.Query;
using TaskManager.Api.Application.QueryHandlers;
using TaskManager.Api.Application.Services;
using TaskManager.Api.Domain.Exceptions;
using TaskManager.Api.Domain.Repository;
using TaskManager.Api.Infrastructure.Service;
using TaskManager.UnitTests.TestFixtures;

namespace TaskManager.UnitTests.QueryHandlers;

public class GetTaskByIdQueryHandlerTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly ITaskService _service = new TaskService();
    private readonly GetTaskByIdQueryHandler _handler;

    public GetTaskByIdQueryHandlerTests()
    {
        _handler = new GetTaskByIdQueryHandler(_repository, _service);
    }

    [Fact]
    public async Task Returns_detail_dto_when_task_exists()
    {
        var entity = TaskItemFixture.Create(id: 7, title: "Found", description: "Detail body");
        _repository.GetByIdAsync(7, Arg.Any<CancellationToken>()).Returns(entity);

        var dto = await _handler.Handle(new GetTaskByIdQuery(7), CancellationToken.None);

        dto.Id.Should().Be(7);
        dto.Title.Should().Be("Found");
        dto.Description.Should().Be("Detail body");
    }

    [Fact]
    public async Task Throws_NotFound_when_task_does_not_exist()
    {
        _repository.GetByIdAsync(999, Arg.Any<CancellationToken>()).ReturnsNull();

        var act = () => _handler.Handle(new GetTaskByIdQuery(999), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("*999*");
    }
}
