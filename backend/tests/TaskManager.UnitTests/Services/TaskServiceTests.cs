using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TaskManager.Api.Exceptions;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;
using TaskManager.Api.Services;
using TaskManager.UnitTests.TestFixtures;

namespace TaskManager.UnitTests.Services;

public class TaskServiceTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _service = new TaskService(_repository);
    }

    // ----------------------------------------------------------------
    // GetTasksAsync
    // ----------------------------------------------------------------

    [Fact]
    public async Task GetTasksAsync_returns_empty_when_repository_returns_no_items()
    {
        _repository.GetAllAsync(null, null, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<TaskItem>());

        var result = await _service.GetTasksAsync(null, null, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTasksAsync_maps_repository_items_to_dtos()
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

        var result = (await _service.GetTasksAsync(null, null, CancellationToken.None)).ToList();

        result.Should().HaveCount(1);
        var dto = result[0];
        dto.Id.Should().Be(42);
        dto.Title.Should().Be("Refactor module");
        dto.Status.Should().Be("in_progress");
        dto.StatusLabel.Should().Be("En progreso");
        dto.Priority.Should().Be("high");
        dto.PriorityLabel.Should().Be("Alta");
    }

    [Fact]
    public async Task GetTasksAsync_forwards_filters_to_repository()
    {
        _repository.GetAllAsync(2, 4, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<TaskItem>());

        await _service.GetTasksAsync(statusId: 2, priorityId: 4, CancellationToken.None);

        await _repository.Received(1).GetAllAsync(2, 4, Arg.Any<CancellationToken>());
    }

    // ----------------------------------------------------------------
    // GetTaskByIdAsync
    // ----------------------------------------------------------------

    [Fact]
    public async Task GetTaskByIdAsync_returns_dto_when_task_exists()
    {
        var entity = TaskItemFixture.Create(id: 7, title: "Found");
        _repository.GetByIdAsync(7, Arg.Any<CancellationToken>()).Returns(entity);

        var dto = await _service.GetTaskByIdAsync(7, CancellationToken.None);

        dto.Id.Should().Be(7);
        dto.Title.Should().Be("Found");
    }

    [Fact]
    public async Task GetTaskByIdAsync_throws_NotFoundException_when_task_does_not_exist()
    {
        _repository.GetByIdAsync(999, Arg.Any<CancellationToken>()).ReturnsNull();

        var act = () => _service.GetTaskByIdAsync(999, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*999*");
    }
}
