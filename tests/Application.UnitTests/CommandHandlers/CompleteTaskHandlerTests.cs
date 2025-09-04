namespace ToDoApp.Application.UnitTests.CommandHandlers;

using ToDoApp.Application.CommandHandlers;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

public sealed class CompleteTaskHandlerTests
{
    private const string DESCRIPTION = "description";
    private const string TITLE = "title-1";
    private static readonly DateTime COMPLETED_AT = new(year: 2025, month: 9, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 8, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private readonly TaskEntity completedTaskEntity;

    private readonly CompleteTaskHandler handler;
    private readonly NullLogger<CompleteTaskHandler> logger = new();
    private readonly TaskEntity taskEntity;
    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();
    private readonly FakeTimeProvider timeProvider = new();

    public CompleteTaskHandlerTests()
    {
        this.timeProvider.SetUtcNow(new DateTime(year: 2025, month: 9, day: 3, hour: 12, minute: 0, second: 0, DateTimeKind.Utc));
        this.taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
        this.completedTaskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        this.handler = new CompleteTaskHandler(this.logger, this.taskRepository, this.timeProvider);
    }

    [Fact]
    public async Task Handle_Should_CompleteTask_When_TaskExists_WithDate()
    {
        // Arrange
        TaskEntity? expectedEntity = null;

        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity);

        await this.taskRepository.UpdateTaskAsync(Arg.Do<TaskEntity>(task => expectedEntity = task), Arg.Any<CancellationToken>());

        var command = new CompleteTask
        {
            Id = TASK_ID_GUID,
            CompletedAt = COMPLETED_AT,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        this.completedTaskEntity.Complete(COMPLETED_AT);

        expectedEntity.Should()
            .BeEquivalentTo(this.completedTaskEntity)
            ;
    }

    [Fact]
    public async Task Handle_Should_CompleteTask_When_TaskExists_WithoutDate()
    {
        // Arrange
        var date = this.timeProvider.GetUtcNow().DateTime;
        this.completedTaskEntity.Complete(date);

        TaskEntity? expectedEntity = null;

        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity);

        await this.taskRepository.UpdateTaskAsync(Arg.Do<TaskEntity>(task => expectedEntity = task), Arg.Any<CancellationToken>());

        var command = new CompleteTask
        {
            Id = TASK_ID_GUID,
            CompletedAt = null,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        expectedEntity.Should()
            .BeEquivalentTo(this.completedTaskEntity)
            ;
    }
}
