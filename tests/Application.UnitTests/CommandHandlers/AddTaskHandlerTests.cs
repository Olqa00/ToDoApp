namespace ToDoApp.Application.UnitTests.CommandHandlers;

using ToDoApp.Application.CommandHandlers;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;

public sealed class AddTaskHandlerTests
{
    private const string DESCRIPTION = "description";
    private const int PERCENT_COMPLETE = 10;
    private const string TITLE = "title-1";
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 8, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    private static readonly DateTime EXPIRY_DATE_TIME = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private static readonly TaskResult TASK_RESULT = new()
    {
        CreatedAt = CREATED_AT,
        Description = DESCRIPTION,
        ExpiryDateTime = EXPIRY_DATE_TIME,
        Id = TASK_ID.Value,
        Title = TITLE,
        CompletedAt = null,
        IsCompleted = false,
        PercentComplete = PERCENT_COMPLETE,
    };

    private readonly AddTaskHandler handler;
    private readonly NullLogger<AddTaskHandler> logger = new();
    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();
    private readonly FakeTimeProvider timeProvider = new();

    public AddTaskHandlerTests()
    {
        this.timeProvider.SetUtcNow(new DateTime(year: 2025, month: 9, day: 3, hour: 12, minute: 0, second: 0, DateTimeKind.Utc));

        this.handler = new AddTaskHandler(this.logger, this.taskRepository, this.timeProvider);
    }

    [Fact]
    public async Task Handle_Should_AddTask_When_TaskDoesNotExist()
    {
        // Arrange
        TaskEntity? taskEntity = null;

        this.taskRepository.GetTaskByIdAsync(TASK_ID, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<TaskResult?>(null));

        await this.taskRepository.AddTaskAsync(Arg.Do<TaskEntity>(task => taskEntity = task), Arg.Any<CancellationToken>());

        var command = new AddTask
        {
            Description = DESCRIPTION,
            ExpiryDateTime = EXPIRY_DATE_TIME,
            Id = TASK_ID_GUID,
            Title = TITLE,
        };

        // Act
        await this.handler.Handle(command, CancellationToken.None);

        // Assert
        var createdAt = this.timeProvider.GetUtcNow().DateTime;

        var expectedEntity = new TaskEntity(TASK_ID, TITLE, createdAt, DESCRIPTION, EXPIRY_DATE_TIME);

        taskEntity.Should()
            .BeEquivalentTo(expectedEntity)
            ;
    }

    [Fact]
    public async Task Handle_Should_ThrowTaskAlreadyExistsException_When_TaskAlreadyExists()
    {
        // Arrange
        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(TASK_RESULT);

        var command = new AddTask
        {
            Description = DESCRIPTION,
            ExpiryDateTime = EXPIRY_DATE_TIME,
            Id = TASK_ID_GUID,
            Title = TITLE,
        };

        // Act
        var act = async () => await this.handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
                .ThrowAsync<TaskAlreadyExistsException>()
                .WithMessage($"Task with id {TASK_ID.Value} already exists.")
            ;
    }
}
