namespace ToDoApp.IntegrationTests.TaskRepository;

using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Types;
using ToDoApp.Infrastructure.Exceptions;
using ToDoApp.Infrastructure.Services;

public sealed class DeleteTaskTests : IntegrationTestBase
{
    private const string DESCRIPTION = "description-1";
    private const string TITLE = "test-task-1";
    private readonly NullLogger<TaskRepository> logger = new();

    [Fact]
    public async Task Should_DeleteTask()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var taskId = new TaskId(guid);
        var createdAt = DateTime.UtcNow;

        var entity = new TaskEntity(taskId, TITLE, createdAt, DESCRIPTION, createdAt.AddDays(2));

        var repository = new TaskRepository(this.DbContext, this.logger);
        await repository.AddTaskAsync(entity, CancellationToken.None);

        // Act
        await repository.DeleteTaskAsync(taskId, CancellationToken.None);

        // Assert
        var tasks = await repository.GetTasksAsync(CancellationToken.None);

        tasks.Should()
            .BeEmpty()
            ;
    }

    [Fact]
    public async Task Should_Throw_When_TaskNotFound()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var taskId = new TaskId(guid);

        var repository = new TaskRepository(this.DbContext, this.logger);

        // Act
        var act = async () => await repository.DeleteTaskAsync(taskId, CancellationToken.None);

        // Assert
        await act.Should()
                .ThrowAsync<TaskNotFoundException>()
                .WithMessage($"Task with id {taskId.Value} not found.")
            ;
    }
}
