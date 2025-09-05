namespace ToDoApp.Application.UnitTests.Extensions;

using ToDoApp.Application.Commands;
using ToDoApp.Application.Extensions;
using ToDoApp.Domain.Entities;

public sealed class TaskEntityExtensions
{
    private const string DESCRIPTION_1 = "description-1";
    private const string DESCRIPTION_2 = "description-2";
    private const int FULL_PERCENT = 100;
    private const int NEW_PERCENT = 23;
    private const string TITLE_1 = "title-1";
    private const string TITLE_2 = "title-2";
    private static readonly DateTime COMPLETED_AT = new(year: 2025, month: 9, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 8, day: 02, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);

    private static readonly DateTime EXPIRY_DATE_TIME_1 = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME_2 = new(year: 2025, month: 10, day: 11, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid TASK_ID_GUID = Guid.NewGuid();
    private static readonly TaskId TASK_ID = new(TASK_ID_GUID);

    private readonly TaskEntity expectedTaskEntity;
    private readonly TaskEntity taskEntity;

    public TaskEntityExtensions()
    {
        this.taskEntity = new TaskEntity(TASK_ID, TITLE_1, CREATED_AT, DESCRIPTION_1, EXPIRY_DATE_TIME_1);
        this.expectedTaskEntity = new TaskEntity(TASK_ID, TITLE_2, CREATED_AT, DESCRIPTION_2, EXPIRY_DATE_TIME_2);
    }

    [Fact]
    public void UpdateFromCommand_Should_UpdateCompletedEntity()
    {
        // Arrange
        this.expectedTaskEntity.Complete(COMPLETED_AT);

        var command = new UpdateTask
        {
            CompletedAt = COMPLETED_AT,
            Description = DESCRIPTION_2,
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
            Id = TASK_ID_GUID,
            PercentComplete = FULL_PERCENT,
            Title = TITLE_2,
        };

        // Act
        var updatedEntity = this.taskEntity.UpdateFromCommand(command);

        // Assert
        updatedEntity.Should()
            .BeEquivalentTo(this.expectedTaskEntity)
            ;
    }

    [Fact]
    public void UpdateFromCommand_Should_UpdateNotCompletedEntity()
    {
        // Arrange
        this.expectedTaskEntity.SetPercentComplete(NEW_PERCENT, completedAt: null);

        var command = new UpdateTask
        {
            Description = DESCRIPTION_2,
            ExpiryDateTime = EXPIRY_DATE_TIME_2,
            Id = TASK_ID_GUID,
            PercentComplete = NEW_PERCENT,
            Title = TITLE_2,
        };

        // Act
        var updatedEntity = this.taskEntity.UpdateFromCommand(command);

        // Assert
        updatedEntity.Should()
            .BeEquivalentTo(this.expectedTaskEntity)
            ;
    }
}
