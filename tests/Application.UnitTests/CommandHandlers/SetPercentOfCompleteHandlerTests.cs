namespace ToDoApp.Application.UnitTests.CommandHandlers;

using ToDoApp.Application.CommandHandlers;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

public sealed class SetPercentOfCompleteHandlerTests
{
    private const string DESCRIPTION = "description";
    private const int FULL_PERCENT = 100;
    private const int NEW_PERCENT = 23;
    private const string TITLE = "title-1";
    private static readonly DateTime COMPLETED_AT = new(year: 2025, month: 9, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 8, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private readonly TaskEntity expectedTaskEntity;

    private readonly SetPercentOfCompleteHandler handler;
    private readonly NullLogger<SetPercentOfCompleteHandler> logger = new();
    private readonly TaskEntity taskEntity;
    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();
    private readonly FakeTimeProvider timeProvider = new();

    public SetPercentOfCompleteHandlerTests()
    {
        this.timeProvider.SetUtcNow(new DateTime(year: 2025, month: 9, day: 3, hour: 12, minute: 0, second: 0, DateTimeKind.Utc));
        this.taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
        this.expectedTaskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        this.handler = new SetPercentOfCompleteHandler(this.logger, this.taskRepository, this.timeProvider);
    }

    [Fact]
    public async Task Handle_Should_SetPercentOfComplete_When_TaskExists_WithDate()
    {
        // Arrange
        TaskEntity? actualEntity = null;

        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity);

        await this.taskRepository.UpdateTaskAsync(Arg.Do<TaskEntity>(task => actualEntity = task), Arg.Any<CancellationToken>());

        var command = new SetPercentOfComplete
        {
            Id = TASK_ID_GUID,
            PercentOfComplete = NEW_PERCENT,
            CompletedAt = COMPLETED_AT,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        this.expectedTaskEntity.SetPercentComplete(NEW_PERCENT, COMPLETED_AT);

        actualEntity.Should()
            .BeEquivalentTo(this.expectedTaskEntity)
            ;
    }

    [Fact]
    public async Task Handle_Should_SetPercentOfComplete_When_TaskExists_WithoutDate()
    {
        // Arrange
        var date = this.timeProvider.GetUtcNow().DateTime;

        TaskEntity? actualEntity = null;

        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity);

        await this.taskRepository.UpdateTaskAsync(Arg.Do<TaskEntity>(task => actualEntity = task), Arg.Any<CancellationToken>());

        var command = new SetPercentOfComplete
        {
            Id = TASK_ID_GUID,
            PercentOfComplete = FULL_PERCENT,
            CompletedAt = null,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);
        // Assert
        this.expectedTaskEntity.SetPercentComplete(FULL_PERCENT, date);

        actualEntity.Should()
            .BeEquivalentTo(this.expectedTaskEntity)
            ;
    }
}
