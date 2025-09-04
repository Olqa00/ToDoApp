namespace ToDoApp.Infrastructure.UnitTests.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.QueryHandlers;

public sealed class GetTomorrowTasksHandlerTests
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

    private readonly GetTomorrowTasksHandler handler;
    private readonly NullLogger<GetTomorrowTasksHandler> logger = new();
    private readonly TaskEntity taskEntity;

    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();
    private readonly FakeTimeProvider timeProvider = new();

    public GetTomorrowTasksHandlerTests()
    {
        this.timeProvider.SetUtcNow(new DateTime(year: 2025, month: 9, day: 4, hour: 12, minute: 0, second: 0, DateTimeKind.Utc));

        this.taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        this.handler = new GetTomorrowTasksHandler(this.logger, this.taskRepository, this.timeProvider);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmptyList_When_NoToDoTasks()
    {
        // Arrange
        this.taskRepository.GetTasksDueOnDayAsync(Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(new List<TaskEntity>());

        var query = new GetTomorrowTasks();

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeEmpty()
            ;
    }

    [Fact]
    public async Task Handle_Should_ReturnListOfToDoTasks_When_ThereAreToDoTasks()
    {
        // Arrange
        this.taskRepository.GetTasksDueOnDayAsync(Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(new List<TaskEntity> { this.taskEntity });

        var query = new GetTomorrowTasks();

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .HaveCount(1)
            .And
            .ContainEquivalentOf(TASK_RESULT)
            ;
    }
}
