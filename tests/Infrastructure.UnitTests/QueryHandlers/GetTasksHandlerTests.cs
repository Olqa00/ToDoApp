namespace ToDoApp.Infrastructure.UnitTests.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Infrastructure.QueryHandlers;

public sealed class GetTasksHandlerTests
{
    private const string DESCRIPTION_1 = "description-1";
    private const string DESCRIPTION_2 = "description-2";
    private const int PERCENT_1 = 100;
    private const int PERCENT_2 = 50;

    private const string TITLE_1 = "title-1";
    private const string TITLE_2 = "title-2";
    private static readonly DateTime COMPETED_AT_1 = new(year: 2025, month: 10, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT_1 = new(year: 2025, month: 10, day: 04, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT_2 = new(year: 2025, month: 10, day: 03, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME_1 = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME_2 = new(year: 2025, month: 11, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    private static readonly Guid TASK_ID_GUID_1 = Guid.NewGuid();
    private static readonly Guid TASK_ID_GUID_2 = Guid.NewGuid();

    private static readonly TaskResult TASK_RESULT_1 = new()
    {
        CompletedAt = COMPETED_AT_1,
        CreatedAt = CREATED_AT_1,
        Description = DESCRIPTION_1,
        ExpiryDateTime = EXPIRY_DATE_TIME_1,
        Id = TASK_ID_GUID_1,
        IsCompleted = true,
        PercentComplete = PERCENT_1,
        Title = TITLE_1,
    };

    private static readonly TaskResult TASK_RESULT_2 = new()
    {
        CompletedAt = null,
        CreatedAt = CREATED_AT_2,
        Description = DESCRIPTION_2,
        ExpiryDateTime = EXPIRY_DATE_TIME_2,
        Id = TASK_ID_GUID_2,
        IsCompleted = false,
        PercentComplete = PERCENT_2,
        Title = TITLE_2,
    };

    private static readonly List<TaskResult> TASK_RESULTS =
    [
        TASK_RESULT_1,
        TASK_RESULT_2,
    ];

    private readonly GetTasksHandler handler;
    private readonly NullLogger<GetTasksHandler> logger = new();
    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();

    public GetTasksHandlerTests()
    {
        this.handler = new GetTasksHandler(this.logger, this.taskRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_EmptyList_When_NoTasks()
    {
        // Arrange
        this.taskRepository.GetTasksAsync(Arg.Any<CancellationToken>())
            .Returns(new List<TaskResult>());

        var query = new GetTasks();

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeEmpty()
            ;
    }

    [Fact]
    public async Task Handle_Should_ReturnTasks()
    {
        // Arrange
        this.taskRepository.GetTasksAsync(Arg.Any<CancellationToken>())
            .Returns(TASK_RESULTS);

        var query = new GetTasks();

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .HaveCount(2)
            .And
            .BeEquivalentTo(TASK_RESULTS)
            ;
    }
}
