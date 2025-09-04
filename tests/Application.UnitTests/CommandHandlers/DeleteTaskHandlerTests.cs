namespace ToDoApp.Application.UnitTests.CommandHandlers;

using ToDoApp.Application.CommandHandlers;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

public sealed class DeleteTaskHandlerTests
{
    private const string DESCRIPTION = "description-1";
    private const int PERCENT = 23;
    private const string TITLE = "title-1";
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 8, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    private static readonly DateTime EXPIRY_DATE_TIME = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private readonly DeleteTaskHandler handler;
    private readonly NullLogger<DeleteTaskHandler> logger = new();
    private readonly TaskEntity taskEntity;
    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();

    public DeleteTaskHandlerTests()
    {
        this.taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
        this.taskEntity.SetPercentComplete(PERCENT, completedAt: null);
        this.handler = new DeleteTaskHandler(this.logger, this.taskRepository);
    }

    [Fact]
    public async Task Handle_Should_DeleteTask_When_TaskExists()
    {
        // Arrange
        TaskId? taskId = null;

        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity);

        await this.taskRepository.DeleteTaskAsync(Arg.Do<TaskId>(task => taskId = task), Arg.Any<CancellationToken>());

        var command = new DeleteTask
        {
            Id = TASK_ID_GUID,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        taskId.Should()
            .BeEquivalentTo(TASK_ID)
            ;
    }

    [Fact]
    public async Task Handle_Should_ThrowTaskNotFoundException_When_TaskDoesNotExist()
    {
        // Arrange
        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns((TaskEntity?)null);

        var command = new DeleteTask
        {
            Id = TASK_ID_GUID,
        };

        // Act
        var act = async () => await this.handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
                .ThrowAsync<TaskNotFoundException>()
                .WithMessage($"Task with id {TASK_ID.Value} not found.")
            ;
    }
}
