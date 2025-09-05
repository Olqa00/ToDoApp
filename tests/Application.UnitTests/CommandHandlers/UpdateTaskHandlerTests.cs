namespace ToDoApp.Application.UnitTests.CommandHandlers;

using ToDoApp.Application.CommandHandlers;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

public sealed class UpdateTaskHandlerTests
{
    private const string DESCRIPTION_1 = "description-1";
    private const string DESCRIPTION_2 = "description-2";
    private const int FULL_PERCENT = 100;
    private const int NEW_PERCENT = 23;
    private const string TITLE_1 = "title-1";
    private const string TITLE_2 = "title-2";
    private static readonly DateTime COMPLETED_AT = new(year: 2025, month: 9, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 8, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    private static readonly DateTime EXPIRY_DATE_TIME_1 = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME_2 = new(year: 2025, month: 10, day: 11, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private readonly TaskEntity expectedTaskEntity;

    private readonly UpdateTaskHandler handler;
    private readonly NullLogger<UpdateTaskHandler> logger = new();
    private readonly TaskEntity taskEntity;
    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();

    public UpdateTaskHandlerTests()
    {
        this.taskEntity = new TaskEntity(TASK_ID, TITLE_1, CREATED_AT, DESCRIPTION_1, EXPIRY_DATE_TIME_1);
        this.expectedTaskEntity = new TaskEntity(TASK_ID, TITLE_2, CREATED_AT, DESCRIPTION_2, EXPIRY_DATE_TIME_2);
        this.expectedTaskEntity.SetPercentComplete(NEW_PERCENT, completedAt: null);
        this.handler = new UpdateTaskHandler(this.logger, this.taskRepository);
    }

    [Fact]
    public async Task Handle_Should_ThrowTaskNotFoundException_When_TaskDoesNotExist()
    {
        // Arrange
        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<TaskEntity?>(null));

        var command = new UpdateTask
        {
            Description = DESCRIPTION_2,
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
            Id = TASK_ID_GUID,
            PercentComplete = NEW_PERCENT,
            Title = TITLE_2,
        };

        // Act
        var act = async () => await this.handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
                .ThrowAsync<TaskNotFoundException>()
                .WithMessage($"Task with id {TASK_ID.Value} not found.")
            ;
    }

    [Fact]
    public async Task Handle_Should_UpdateTask_When_TaskExists()
    {
        // Arrange
        TaskEntity? updatedTaskEntity = null;

        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity);

        await this.taskRepository.UpdateTaskAsync(Arg.Do<TaskEntity>(task => updatedTaskEntity = task), Arg.Any<CancellationToken>());

        var command = new UpdateTask
        {
            Description = DESCRIPTION_2,
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
            Id = TASK_ID_GUID,
            PercentComplete = NEW_PERCENT,
            Title = TITLE_2,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        updatedTaskEntity.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(this.expectedTaskEntity)
            ;
    }

    [Fact]
    public async Task Handle_Should_UpdateTask_When_TaskExistsAndIsCompleted()
    {
        // Arrange
        this.expectedTaskEntity.Complete(COMPLETED_AT);

        TaskEntity? updatedTaskEntity = null;

        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity);

        await this.taskRepository.UpdateTaskAsync(Arg.Do<TaskEntity>(task => updatedTaskEntity = task), Arg.Any<CancellationToken>());

        var command = new UpdateTask
        {
            CompletedAt = COMPLETED_AT,
            Description = DESCRIPTION_2,
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
            Id = TASK_ID_GUID,
            PercentComplete = FULL_PERCENT,
            Title = TITLE_2,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        updatedTaskEntity.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(this.expectedTaskEntity)
            ;
    }
}
