namespace ToDoApp.Infrastructure.UnitTests.Extensions;

using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.Extensions;
using ToDoApp.Infrastructure.Models;

public class TaskEntityExtensionsTests
{
    private const string DESCRIPTION_1 = "description-1";
    private const string DESCRIPTION_2 = "description-2";
    private const int PERCENT_0 = 0;
    private const int PERCENT_FULL = 100;

    private const string TITLE_1 = "title-1";
    private const string TITLE_2 = "title-2";
    private static readonly DateTime COMPETED_AT = new(year: 2025, month: 10, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 10, day: 04, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    private static readonly Guid TASK_ID_GUID_1 = Guid.NewGuid();
    private static readonly TaskId TASK_ID_1 = new(TASK_ID_GUID_1);
    private static readonly Guid TASK_ID_GUID_2 = Guid.NewGuid();
    private static readonly TaskId TASK_ID_2 = new(TASK_ID_GUID_2);

    private static readonly TaskResult TASK_RESULT_COMPLETED = new()
    {
        CompletedAt = COMPETED_AT,
        CreatedAt = CREATED_AT,
        Description = DESCRIPTION_1,
        ExpiryDateTime = EXPIRY_DATE_TIME,
        Id = TASK_ID_GUID_1,
        IsCompleted = true,
        PercentComplete = PERCENT_FULL,
        Title = TITLE_1,
    };

    private static readonly TaskResult TASK_RESULT_NOT_COMPLETED = new()
    {
        CompletedAt = null,
        CreatedAt = CREATED_AT,
        Description = DESCRIPTION_2,
        ExpiryDateTime = EXPIRY_DATE_TIME,
        Id = TASK_ID_GUID_2,
        IsCompleted = false,
        PercentComplete = PERCENT_0,
        Title = TITLE_2,
    };

    private static readonly TaskDbModel TASK_DB_MODEL_COMPLETED = new()
    {
        CompletedAt = COMPETED_AT,
        CreatedAt = CREATED_AT,
        Description = DESCRIPTION_1,
        ExpiryDateTime = EXPIRY_DATE_TIME,
        Id = TASK_ID_1.Value,
        PercentComplete = PERCENT_FULL,
        Title = TITLE_1,
    };

    private static readonly TaskDbModel TASK_DB_MODEL_NOT_COMPLETED = new()
    {
        CompletedAt = null,
        CreatedAt = CREATED_AT,
        Description = DESCRIPTION_2,
        ExpiryDateTime = EXPIRY_DATE_TIME,
        Id = TASK_ID_2.Value,
        PercentComplete = PERCENT_0,
        Title = TITLE_2,
    };

    private static readonly List<TaskResult> TASK_RESULTS =
    [
        TASK_RESULT_NOT_COMPLETED,
        TASK_RESULT_COMPLETED,
    ];

    private readonly TaskEntity taskEntity1;
    private readonly TaskEntity taskEntity2;

    public TaskEntityExtensionsTests()
    {
        this.taskEntity1 = new TaskEntity(TASK_ID_1, TITLE_1, CREATED_AT, DESCRIPTION_1, EXPIRY_DATE_TIME);
        this.taskEntity2 = new TaskEntity(TASK_ID_2, TITLE_2, CREATED_AT, DESCRIPTION_2, EXPIRY_DATE_TIME);

        this.taskEntity1.Complete(COMPETED_AT);
    }

    [Fact]
    public void ToDbModel_Should_MapCompletedEntity_ToDbModel()
    {
        // Arrange

        // Act
        var result = this.taskEntity1.ToDbModel();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(TASK_DB_MODEL_COMPLETED)
            ;
    }

    [Fact]
    public void ToDbModel_Should_MapEntity_ToDbModel()
    {
        // Arrange

        // Act
        var result = this.taskEntity2.ToDbModel();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(TASK_DB_MODEL_NOT_COMPLETED)
            ;
    }

    [Fact]
    public void ToResult_Should_MapCompletedEntity_ToResult()
    {
        // Arrange

        // Act
        var result = this.taskEntity1.ToResult();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(TASK_RESULT_COMPLETED)
            ;
    }

    [Fact]
    public void ToResults_Should_MapEntities()
    {
        // Arrange
        var entities = new List<TaskEntity>
        {
            this.taskEntity1,
            this.taskEntity2,
        };

        // Act
        var result = entities.ToResults();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(TASK_RESULTS)
            ;
    }
}
