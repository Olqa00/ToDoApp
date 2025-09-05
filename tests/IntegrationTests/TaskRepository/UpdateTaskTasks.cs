namespace ToDoApp.IntegrationTests.TaskRepository;

using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Types;
using ToDoApp.Infrastructure.Exceptions;
using ToDoApp.Infrastructure.Services;

public sealed class UpdateTaskTasks : IntegrationTestBase
{
    private const string DESCRIPTION_1 = "description-1";
    private const string DESCRIPTION_2 = "description-2";
    private const string TITLE_1 = "test-task-1";
    private const string TITLE_2 = "test-task-2";
    private readonly NullLogger<TaskRepository> logger = new();

    [Fact]
    public async Task Should_Throw_When_TryToUpdateNotExistingTask()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var taskId = new TaskId(guid);
        var createdAt = DateTime.UtcNow;
        var entity = new TaskEntity(taskId, TITLE_1, createdAt, DESCRIPTION_1, createdAt.AddDays(2));
        var repository = new TaskRepository(this.DbContext, this.logger);

        // Act
        var act = async () => await repository.UpdateTaskAsync(entity, CancellationToken.None);

        // Assert
        await act.Should()
                .ThrowAsync<TaskNotFoundException>()
                .WithMessage($"Task with id {taskId.Value} not found.")
            ;
    }

    [Fact]
    public async Task Should_UpdateTask()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var taskId = new TaskId(guid);
        var createdAt = DateTime.UtcNow;
        var entity = new TaskEntity(taskId, TITLE_1, createdAt, DESCRIPTION_1, createdAt.AddDays(2));
        var repository = new TaskRepository(this.DbContext, this.logger);
        await repository.AddTaskAsync(entity, CancellationToken.None);

        var updatedEntity = new TaskEntity(taskId, TITLE_2, createdAt, DESCRIPTION_2, createdAt.AddDays(3));
        updatedEntity.Complete(null);

        // Act
        await repository.UpdateTaskAsync(updatedEntity, CancellationToken.None);

        // Assert
        var tasks = await repository.GetTasksAsync(CancellationToken.None);

        var dbTask = tasks.Should()
                .ContainSingle(task => task.Id.Value == taskId.Value)
                .Subject
            ;

        dbTask.Should()
            .BeEquivalentTo(updatedEntity)
            ;
    }
}
