namespace ToDoApp.IntegrationTests.TaskRepository;

using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Types;
using ToDoApp.Infrastructure.Services;

public sealed class AddTaskTests : IntegrationTestBase
{
    private const string DESCRIPTION = "description-1";
    private const string TITLE = "test-task-1";
    private readonly NullLogger<TaskRepository> logger = new();

    [Fact]
    public async Task Should_AddTask()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var taskId = new TaskId(guid);
        var createdAt = DateTime.UtcNow;
        var entity = new TaskEntity(taskId, TITLE, createdAt, DESCRIPTION, createdAt.AddDays(2));

        var repository = new TaskRepository(this.DbContext, this.logger);

        // Act
        await repository.AddTaskAsync(entity, CancellationToken.None);
        await this.DbContext.SaveChangesAsync();

        // Assert
        var tasks = await repository.GetTasksAsync(CancellationToken.None);

        var dbTask = tasks.Should()
                .ContainSingle(task => task.Id.Value == taskId.Value)
                .Subject
            ;

        dbTask.Should()
            .BeEquivalentTo(entity)
            ;
    }
}
