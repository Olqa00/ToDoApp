namespace ToDoApp.Domain.UnitTests.Entities;

using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Exceptions;

public sealed class TaskEntityTests
{
    private const string DESCRIPTION = "This is a test task.";
    private const int FULL_PERCENT = 100;
    private const string NEW_DESCRIPTION = "This is a new test task.";
    private const string NEW_TITLE = "New Test Task";
    private const int PERCENT = 50;
    private const string TITLE = "Test Task";
    private static readonly DateTime COMPETED_AT_1 = new(year: 2025, month: 10, day: 05, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime COMPETED_AT_2 = new(year: 2025, month: 10, day: 06, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime CREATED_AT = new(year: 2025, month: 10, day: 04, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly DateTime EXPIRY_DATE_TIME = new(year: 2025, month: 10, day: 10, hour: 10, minute: 0, second: 0, DateTimeKind.Utc);
    private static readonly Guid ID_GUID = Guid.NewGuid();
    private static readonly TaskId ID = new(ID_GUID);

    private static readonly DateTime WRONG_DATE = new(year: 2025, month: 10, day: 1, hour: 1, minute: 0, second: 0, DateTimeKind.Utc);

    [Fact]
    public void Complete_Should_SetCompletedAt_When_TaskIsNotCompleted()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        task.Complete(COMPETED_AT_1);

        // Assert
        task.CompletedAt.Should()
            .Be(COMPETED_AT_1)
            ;
    }

    [Fact]
    public void Complete_Should_ThrowException_When_TaskAlreadyCompleted()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
        task.Complete(COMPETED_AT_1);

        // Act
        var act = () => task.Complete(COMPETED_AT_2);

        // Assert
        act.Should()
            .Throw<TaskAlreadyCompletedException>()
            .WithMessage($"The task {ID.Value} has already been completed.")
            ;
    }

    [Fact]
    public void Complete_Should_ThrowException_When_TaskCompletedBeforeCreation()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        var act = () => task.Complete(WRONG_DATE);

        // Assert
        act.Should()
            .Throw<TaskCompletedBeforeCreationException>()
            .WithMessage($"The task {ID.Value} was completed before it was created.")
            ;
    }

    [Fact]
    public void Constructor_Should_SetProperties()
    {
        // Arrange

        // Act
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Assert
        task.Id.Should()
            .Be(ID)
            ;

        task.Title.Should()
            .Be(TITLE)
            ;

        task.CreatedAt.Should()
            .Be(CREATED_AT)
            ;

        task.Description.Should()
            .Be(DESCRIPTION)
            ;

        task.ExpiryDateTime.Should()
            .Be(EXPIRY_DATE_TIME)
            ;
    }

    [Fact]
    public void Reschedule_Should_SetNewExpiryDate_When_NewExpiryIsValid()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
        var newExpiry = EXPIRY_DATE_TIME.AddDays(5);

        // Act
        task.Reschedule(newExpiry);

        // Assert
        task.ExpiryDateTime.Should()
            .Be(newExpiry)
            ;
    }

    [Fact]
    public void Reschedule_Should_ThrowException_When_NewExpiryIsBeforeCreationDate()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
        var newExpiry = CREATED_AT.AddDays(-1);

        // Act
        var act = () => task.Reschedule(newExpiry);

        // Assert
        act.Should()
            .Throw<TaskInvalidExpiryDateException>()
            .WithMessage($"Task with id '{ID.Value}' cannot have due date earlier than its creation date.")
            ;
    }

    [Fact]
    public void SetDescription_Should_SetNewDescription_When_DescriptionIsValid()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        task.SetDescription(NEW_DESCRIPTION);

        // Assert
        task.Description.Should()
            .Be(NEW_DESCRIPTION)
            ;
    }

    [Theory, InlineData(""), InlineData("   ")]
    public void SetDescription_Should_ThrowException_When_DescriptionIsEmpty(string invalidDescription)
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        var act = () => task.SetDescription(invalidDescription);

        // Assert
        act.Should()
            .Throw<TaskEmptyDescriptionException>()
            .WithMessage($"The Task {ID.Value} has empty description.")
            ;
    }

    [Fact]
    public void SetPercentComplete_Should_SetCompletedAt_When_PercentIs100()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        task.SetPercentComplete(FULL_PERCENT, COMPETED_AT_1);

        // Assert
        task.PercentComplete.Should()
            .Be(FULL_PERCENT)
            ;

        task.CompletedAt.Should()
            .Be(COMPETED_AT_1)
            ;
    }

    [Fact]
    public void SetPercentComplete_Should_SetNewPercent_When_PercentIsValid()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        task.SetPercentComplete(PERCENT, COMPETED_AT_1);

        // Assert
        task.PercentComplete.Should()
            .Be(PERCENT)
            ;

        task.CompletedAt.Should()
            .BeNull()
            ;
    }

    [Theory, InlineData(-2), InlineData(101)]
    public void SetPercentComplete_Should_ThrowException_When_PercentIsOutOfRange(int invalidPercent)
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        var act = () => task.SetPercentComplete(invalidPercent, COMPETED_AT_1);

        // Assert
        act.Should()
            .Throw<TaskInvalidPercentException>()
            .WithMessage($"Task with id '{ID.Value}' cannot have percent complete set to {invalidPercent}. Allowed range is 0–100.")
            ;
    }

    [Fact]
    public void SetTitle_Should_SetNewTitle_When_TitleIsValid()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        task.SetTitle(NEW_TITLE);

        // Assert
        task.Title.Should()
            .Be(NEW_TITLE)
            ;
    }

    [Theory, InlineData(""), InlineData("   ")]
    public void SetTitle_Should_ThrowException_When_TitleIsEmpty(string invalidTitle)
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        var act = () => task.SetTitle(invalidTitle);

        // Assert
        act.Should()
            .Throw<TaskEmptyTitleException>()
            .WithMessage($"The Task {ID.Value} has empty title.")
            ;
    }

    [Theory, InlineData(-2), InlineData(101)]
    public void UnComplete_Should_ThrowException_When_PercentIsOutOfRange(int invalidPercent)
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
        task.Complete(COMPETED_AT_1);

        // Act
        var act = () => task.UnComplete(invalidPercent);

        // Assert
        act.Should()
            .Throw<TaskInvalidPercentException>()
            .WithMessage($"Task with id '{ID.Value}' cannot have percent complete set to {invalidPercent}. Allowed range is 0–100.")
            ;
    }

    [Fact]
    public void UnComplete_Should_ThrowException_When_TaskIsNotCompleted()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);

        // Act
        var act = () => task.UnComplete(PERCENT);

        // Assert
        act.Should()
            .Throw<TaskNotCompletedException>()
            .WithMessage($"The task {ID.Value} has not been completed.")
            ;
    }

    [Fact]
    public void UnComplete_Should_UnCompleteTask()
    {
        // Arrange
        var task = new TaskEntity(ID, TITLE, CREATED_AT, DESCRIPTION, EXPIRY_DATE_TIME);
        task.Complete(COMPETED_AT_1);

        // Act
        task.UnComplete(null);

        // Assert
        task.CompletedAt.Should()
            .BeNull()
            ;

        task.PercentComplete.Should()
            .Be(0)
            ;
    }
}
