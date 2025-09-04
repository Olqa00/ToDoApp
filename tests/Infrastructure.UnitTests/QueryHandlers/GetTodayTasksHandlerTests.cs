namespace ToDoApp.Infrastructure.UnitTests.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.QueryHandlers;

public sealed class GetTodayTasksHandlerTests
{
    private const string DESCRIPTION = "description";
    private const int PERCENT_0 = 0;
    private const string TITLE = "title-1";
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 8, day: 2, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME = new(year: 2025, month: 9, day: 4, hour: 16, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private static readonly TaskResult TASK_RESULT = new()
    {
        CompletedAt = null,
        CreatedAt = CREATED_AT,
        Description = DESCRIPTION,
        ExpiryDateTime = EXPIRY_DATE_TIME,
        Id = TASK_ID_GUID,
        IsCompleted = false,
        PercentComplete = PERCENT_0,
        Title = TITLE,
    };

    private readonly GetTodayTasksHandler handler;
    private readonly NullLogger<GetTodayTasksHandler> logger = new();
    private readonly TaskEntity taskEntity;

    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();
    private readonly FakeTimeProvider timeProvider = new();

    public GetTodayTasksHandlerTests()
    {
        this.timeProvider.SetUtcNow(new DateTime(year: 2025, month: 9, day: 4, hour: 12, minute: 0, second: 0, DateTimeKind.Utc));

        this.taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        this.handler = new GetTodayTasksHandler(this.logger, this.taskRepository, this.timeProvider);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyList_When_NoToDoTasks()
    {
        // Arrange
        this.taskRepository.GetTasksDueOnDay(Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(new List<TaskEntity>());

        var query = new GetTodayTasks();

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeEmpty()
            ;
    }

    [Fact]
    public async Task Handle_Should_ReturnTasks_When_TasksExist()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            this.taskEntity,
        };

        var results = new List<TaskResult>
        {
            TASK_RESULT,
        };

        this.taskRepository.GetTasksDueOnDay(Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(tasks);

        var query = new GetTodayTasks();

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeEquivalentTo(results)
            ;
    }
}
