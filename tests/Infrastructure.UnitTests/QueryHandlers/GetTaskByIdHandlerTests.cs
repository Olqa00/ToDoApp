namespace ToDoApp.Infrastructure.UnitTests.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.QueryHandlers;

public sealed class GetTaskByIdHandlerTests
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
    private static readonly TaskId TASK_ID_1 = new(TASK_ID_GUID_1);
    private static readonly Guid TASK_ID_GUID_2 = Guid.NewGuid();
    private static readonly TaskId TASK_ID_2 = new(TASK_ID_GUID_2);

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

    private readonly GetTaskByIdHandler handler;
    private readonly NullLogger<GetTaskByIdHandler> logger = new();
    private readonly TaskEntity taskEntity1;
    private readonly TaskEntity taskEntity2;
    private readonly ITaskRepository taskRepository = Substitute.For<ITaskRepository>();

    public GetTaskByIdHandlerTests()
    {
        this.taskEntity1 = new TaskEntity(TASK_ID_1, TITLE_1, CREATED_AT_1, DESCRIPTION_1, EXPIRY_DATE_TIME_1);
        this.taskEntity1.Complete(COMPETED_AT_1);

        this.taskEntity2 = new TaskEntity(TASK_ID_2, TITLE_2, CREATED_AT_2, DESCRIPTION_2, EXPIRY_DATE_TIME_2);
        this.taskEntity2.SetPercentComplete(PERCENT_2, completedAt: null);

        this.handler = new GetTaskByIdHandler(this.logger, this.taskRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnCompletedTask_WhenTaskExists()
    {
        // Arrange
        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity1);

        var query = new GetTaskById
        {
            Id = TASK_ID_GUID_1,
        };

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeEquivalentTo(TASK_RESULT_1)
            ;
    }

    [Fact]
    public async Task Handle_ShouldReturnIncompleteTask_WhenTaskExists()
    {
        // Arrange
        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns(this.taskEntity2);

        var query = new GetTaskById
        {
            Id = TASK_ID_GUID_2,
        };

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeEquivalentTo(TASK_RESULT_2)
            ;
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Arrange
        this.taskRepository.GetTaskByIdAsync(Arg.Any<TaskId>(), Arg.Any<CancellationToken>())
            .Returns((TaskEntity?)null);

        var query = new GetTaskById
        {
            Id = TASK_ID_GUID_1,
        };

        // Act
        var result = await this.handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should()
            .BeNull()
            ;
    }
}
