namespace ToDoApp.Infrastructure.UnitTests.Extensions;

using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.Extensions;
using ToDoApp.Infrastructure.Models;

public sealed class TaskDbModelExtensionsTests
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

    private static readonly TaskDbModel TASK_DB_MODEL_1 = new()
    {
        CompletedAt = COMPETED_AT_1,
        CreatedAt = CREATED_AT_1,
        Description = DESCRIPTION_1,
        ExpiryDateTime = EXPIRY_DATE_TIME_1,
        Id = TASK_ID_GUID_1,
        PercentComplete = PERCENT_1,
        Title = TITLE_1,
    };

    private static readonly TaskDbModel TASK_DB_MODEL_2 = new()
    {
        CompletedAt = null,
        CreatedAt = CREATED_AT_2,
        Description = DESCRIPTION_2,
        ExpiryDateTime = EXPIRY_DATE_TIME_2,
        Id = TASK_ID_GUID_2,
        PercentComplete = PERCENT_2,
        Title = TITLE_2,
    };

    private static readonly List<TaskDbModel> TASK_DB_MODELS =
    [
        TASK_DB_MODEL_1,
        TASK_DB_MODEL_2,
    ];

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

    private readonly TaskEntity taskEntity1;
    private readonly TaskEntity taskEntity2;

    public TaskDbModelExtensionsTests()
    {
        this.taskEntity1 = new TaskEntity(TASK_ID_1, TITLE_1, CREATED_AT_1, DESCRIPTION_1, EXPIRY_DATE_TIME_1);
        this.taskEntity2 = new TaskEntity(TASK_ID_2, TITLE_2, CREATED_AT_2, DESCRIPTION_2, EXPIRY_DATE_TIME_2);

        this.taskEntity1.Complete(COMPETED_AT_1);
        this.taskEntity2.SetPercentComplete(PERCENT_2, completedAt: null);
    }

    [Fact]
    public void ToEntities_Should_ReturnEntities()
    {
        // Arrange
        var entities = new List<TaskEntity>
        {
            this.taskEntity1,
            this.taskEntity2,
        };

        // Act
        var result = TASK_DB_MODELS.ToEntities();

        // Assert
        result.Should()
            .BeEquivalentTo(entities)
            ;
    }

    [Fact]
    public void ToEntity_Should_ReturnEntity()
    {
        // Arrange

        // Act
        var result = TASK_DB_MODEL_1.ToEntity();

        // Assert
        result.Should()
            .BeEquivalentTo(this.taskEntity1)
            ;
    }

    [Fact]
    public void ToResult_Should_ReturnResult()
    {
        // Arrange

        // Act
        var result = TASK_DB_MODEL_1.ToResult();

        // Assert
        result.Should()
            .BeEquivalentTo(TASK_RESULT_1)
            ;
    }

    [Fact]
    public void ToResults_Should_ReturnResults()
    {
        // Arrange

        // Act
        var result = TASK_DB_MODELS.ToResults();

        // Assert
        result.Should()
            .BeEquivalentTo(TASK_RESULTS)
            ;
    }

    [Fact]
    public void UpdateDbModel_Should_UpdateCompletedDbModel()
    {
        // Arrange
        var expectedDbModel = new TaskDbModel
        {
            CompletedAt = COMPETED_AT_1,
            CreatedAt = CREATED_AT_2,
            Description = DESCRIPTION_2,
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
            Id = TASK_ID_GUID_1,
            PercentComplete = PERCENT_1,
            Title = TITLE_2,
        };

        var newEntity = new TaskEntity(TASK_ID_1, TITLE_2, CREATED_AT_2, DESCRIPTION_2, EXPIRY_DATE_TIME_2);
        newEntity.SetPercentComplete(PERCENT_1, COMPETED_AT_1);

        // Act
        var result = TASK_DB_MODEL_1.UpdateDbModel(newEntity);

        // Assert
        result.Should()
            .BeEquivalentTo(expectedDbModel)
            ;
    }

    [Fact]
    public void UpdateDbModel_Should_UpdateDbModel()
    {
        // Arrange
        var expectedDbModel = new TaskDbModel
        {
            CreatedAt = CREATED_AT_2,
            Description = DESCRIPTION_2,
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
            Id = TASK_ID_GUID_1,
            PercentComplete = PERCENT_2,
            Title = TITLE_2,
        };

        var newEntity = new TaskEntity(TASK_ID_1, TITLE_2, CREATED_AT_2, DESCRIPTION_2, EXPIRY_DATE_TIME_2);
        newEntity.SetPercentComplete(PERCENT_2, completedAt: null);

        // Act
        var result = TASK_DB_MODEL_1.UpdateDbModel(newEntity);

        // Assert
        result.Should()
            .BeEquivalentTo(expectedDbModel)
            ;
    }
}
