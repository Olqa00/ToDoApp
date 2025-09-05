namespace ToDoApp.Application.UnitTests.CommandHandlers;

using ToDoApp.Application.CommandHandlers;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

public sealed class RescheduleTaskHandlerTests
{
    private const string DESCRIPTION = "description-1";
    private const int PERCENT = 23;
    private const string TITLE = "title-1";
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 8, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    private static readonly DateTime EXPIRY_DATE_TIME_1 = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME_2 = new(year: 2025, month: 10, day: 11, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private readonly RescheduleTaskHandler handler;
    private readonly NullLogger<RescheduleTaskHandler> logger = new();
    private readonly TaskEntity taskEntity;
    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();

    public RescheduleTaskHandlerTests()
    {
        this.taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME_1);
        this.taskEntity.SetPercentComplete(PERCENT, completedAt: null);
        this.handler = new RescheduleTaskHandler(this.logger, this.taskRepository);
    }

    [Fact]
    public async Task Handle_Should_RescheduleTask_When_TaskExists()
    {
        // Arrange
        TaskEntity? updatedTaskEntity = null;

        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity);

        var command = new RescheduleTask
        {
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
            Id = TASK_ID_GUID,
        };

        await this.taskRepository.UpdateTaskAsync(Arg.Do<TaskEntity>(task => updatedTaskEntity = task), Arg.Any<CancellationToken>());

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        var expectedEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME_2);
        expectedEntity.SetPercentComplete(PERCENT, completedAt: null);

        updatedTaskEntity.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedEntity)
            ;
    }

    [Fact]
    public async Task Handle_Should_ThrowTaskNotFoundException_When_TaskDoesNotExist()
    {
        // Arrange
        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<TaskEntity?>(null));

        var command = new RescheduleTask
        {
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
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
