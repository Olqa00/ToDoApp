namespace ToDoApp.Infrastructure.UnitTests.Extensions;

using ToDoApp.Application.Results;
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
    private static readonly Guid TASK_ID_GUID_2 = Guid.NewGuid();

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
}
