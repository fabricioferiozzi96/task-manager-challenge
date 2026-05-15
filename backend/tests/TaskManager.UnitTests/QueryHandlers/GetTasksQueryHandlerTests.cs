using FluentAssertions;
using NSubstitute;
using TaskManager.Api.Application.Dtos;
using TaskManager.Api.Application.Query;
using TaskManager.Api.Application.QueryHandlers;
using TaskManager.Api.Application.Services;
using TaskManager.Api.Domain.Entities;
using TaskManager.Api.Domain.Repository;
using TaskManager.Api.Infrastructure.Service;
using TaskManager.UnitTests.TestFixtures;

namespace TaskManager.UnitTests.QueryHandlers;

public class GetTasksQueryHandlerTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly ITaskService _service = new TaskService(); // mapeo puro, sin mock
    private readonly GetTasksQueryHandler _handler;

    public GetTasksQueryHandlerTests()
    {
        _handler = new GetTasksQueryHandler(_repository, _service);
    }

    [Fact]
    public async Task Returns_empty_when_repository_returns_no_items()
    {
        _repository.GetAllAsync(null, null, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<TaskItem>());

        var result = await _handler.Handle(new GetTasksQuery(null, null), CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Maps_repository_entities_to_list_dtos()
    {
        var entity = TaskItemFixture.Create(
            id: 42,
            title: "Refactor module",
            statusCode: "in_progress",
            statusLabel: "En progreso",
            priorityCode: "high",
            priorityLabel: "Alta");

        _repository.GetAllAsync(null, null, Arg.Any<CancellationToken>())
            .Returns(new[] { entity });

        var result = await _handler.Handle(new GetTasksQuery(null, null), CancellationToken.None);

        result.Should().HaveCount(1);
        var dto = result[0];
        dto.Should().BeOfType<TaskListItemDto>();
        dto.Id.Should().Be(42);
        dto.Title.Should().Be("Refactor module");
        dto.Status.Should().Be("En progreso");
        dto.Priority.Should().Be("Alta");
    }

    [Fact]
    public async Task Forwards_filters_to_repository()
    {
        _repository.GetAllAsync(2, 4, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<TaskItem>());

        await _handler.Handle(new GetTasksQuery(StatusId: 2, PriorityId: 4), CancellationToken.None);

        await _repository.Received(1).GetAllAsync(2, 4, Arg.Any<CancellationToken>());
    }
}
