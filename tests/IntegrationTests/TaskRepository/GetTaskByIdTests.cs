namespace ToDoApp.IntegrationTests.TaskRepository;

using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Types;
using ToDoApp.Infrastructure.Services;

public sealed class GetTaskByIdTests : IntegrationTestBase
{
    private const string DESCRIPTION = "description-1";
    private const string TITLE = "test-task-1";
    private readonly NullLogger<TaskRepository> logger = new();

    [Fact]
    public async Task Should_ReturnNull_When_TaskDoesNotExist()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var taskId = new TaskId(guid);

        var repository = new TaskRepository(this.DbContext, this.logger);

        // Act
        var dbTask = await repository.GetTaskByIdAsync(taskId, CancellationToken.None);

        // Assert
        dbTask.Should()
            .BeNull()
            ;
    }

    [Fact]
    public async Task Should_ReturnTask_When_TaskExists()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var taskId = new TaskId(guid);
        var createdAt = DateTime.UtcNow;

        var entity = new TaskEntity(taskId, TITLE, createdAt, DESCRIPTION, createdAt.AddDays(2));

        var repository = new TaskRepository(this.DbContext, this.logger);
        await repository.AddTaskAsync(entity, CancellationToken.None);

        // Act
        var dbTask = await repository.GetTaskByIdAsync(taskId, CancellationToken.None);

        // Assert
        dbTask.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(entity)
            ;
    }
}
