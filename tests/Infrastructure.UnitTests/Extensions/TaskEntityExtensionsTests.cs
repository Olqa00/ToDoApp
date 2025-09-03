namespace ToDoApp.Infrastructure.UnitTests.Extensions;

using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.Extensions;
using ToDoApp.Infrastructure.Models;

public class TaskEntityExtensionsTests
{
    private const string DESCRIPTION = "description-1";
    private const int PERCENT_0 = 0;
    private const int PERCENT_FULL = 100;

    private const string TITLE = "title-1";
    private static readonly DateTime COMPETED_AT = new(year: 2025, month: 10, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 10, day: 04, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private static readonly TaskDbModel TASK_DB_MODEL_COMPLETED = new()
    {
        CompletedAt = COMPETED_AT,
        CreatedAt = CREATED_AT,
        Description = DESCRIPTION,
        ExpiryDateTime = EXPIRY_DATE_TIME,
        Id = TASK_ID.Value,
        PercentComplete = PERCENT_FULL,
        Title = TITLE,
    };

    private static readonly TaskDbModel TASK_DB_MODEL_NOT_COMPLETED = new()
    {
        CompletedAt = null,
        CreatedAt = CREATED_AT,
        Description = DESCRIPTION,
        ExpiryDateTime = EXPIRY_DATE_TIME,
        Id = TASK_ID.Value,
        PercentComplete = PERCENT_0,
        Title = TITLE,
    };

    private readonly TaskEntity taskEntity;

    public TaskEntityExtensionsTests()
    {
        this.taskEntity = new TaskEntity(TASK_ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
    }

    [Fact]
    public void ToDbModel_Should_Map_Completed_Entity_To_DbModel()
    {
        // Arrange
        this.taskEntity.Complete(COMPETED_AT);

        // Act
        var result = this.taskEntity.ToDbModel();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(TASK_DB_MODEL_COMPLETED)
            ;
    }

    [Fact]
    public void ToDbModel_Should_Map_Entity_To_DbModel()
    {
        // Arrange

        // Act
        var result = this.taskEntity.ToDbModel();

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(TASK_DB_MODEL_NOT_COMPLETED)
            ;
    }
}
