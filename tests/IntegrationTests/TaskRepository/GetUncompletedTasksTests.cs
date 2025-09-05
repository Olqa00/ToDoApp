namespace ToDoApp.IntegrationTests.TaskRepository;

using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Types;
using ToDoApp.Infrastructure.Services;

public sealed class GetUncompletedTasksTests : IntegrationTestBase
{
    private const string DESCRIPTION_1 = "description-1";
    private const string DESCRIPTION_2 = "description-2";
    private const string DESCRIPTION_3 = "description-3";
    private const string TITLE_1 = "test-task-1";
    private const string TITLE_2 = "test-task-2";
    private const string TITLE_3 = "test-task-3";
    private readonly NullLogger<TaskRepository> logger = new();

    [Fact]
    public async Task Should_ReturnTwoTasks()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var taskId1 = new TaskId(guid1);
        var taskId2 = new TaskId(guid2);
        var taskId3 = new TaskId(guid3);
        var createdAt = DateTime.UtcNow;

        var task1 = new TaskEntity(taskId1, TITLE_1, createdAt, DESCRIPTION_1, createdAt.AddDays(2));
        var task2 = new TaskEntity(taskId2, TITLE_2, createdAt, DESCRIPTION_2, createdAt.AddDays(3));
        var task3 = new TaskEntity(taskId3, TITLE_3, createdAt, DESCRIPTION_3, createdAt.AddDays(1));
        task3.Complete(null);

        var entities = new List<TaskEntity>
        {
            task1,
            task2,
        };

        var repository = new TaskRepository(this.DbContext, this.logger);

        await repository.AddTaskAsync(task1, CancellationToken.None);
        await repository.AddTaskAsync(task2, CancellationToken.None);
        await repository.AddTaskAsync(task3, CancellationToken.None);

        // Act
        var tasks = await repository.GetUncompletedTasksAsync(CancellationToken.None);

        // Assert
        tasks.Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(entities)
            ;
    }
}
