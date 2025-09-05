namespace ToDoApp.IntegrationTests.TaskRepository;

using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Types;
using ToDoApp.Infrastructure.Services;

public sealed class GetTasksTests : IntegrationTestBase
{
    private const string DESCRIPTION_1 = "description-1";
    private const string DESCRIPTION_2 = "description-2";
    private const string TITLE_1 = "test-task-1";
    private const string TITLE_2 = "test-task-2";
    private readonly NullLogger<TaskRepository> logger = new();

    [Fact]
    public async Task Should_ReturnTwoTasks()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var taskId1 = new TaskId(guid1);
        var taskId2 = new TaskId(guid2);
        var createdAt = DateTime.UtcNow;

        var task1 = new TaskEntity(taskId1, TITLE_1, createdAt, DESCRIPTION_1, createdAt.AddDays(2));
        var task2 = new TaskEntity(taskId2, TITLE_2, createdAt, DESCRIPTION_2, createdAt.AddDays(3));

        var entities = new List<TaskEntity>
        {
            task1,
            task2,
        };

        var repository = new TaskRepository(this.DbContext, this.logger);

        await repository.AddTaskAsync(task1, CancellationToken.None);
        await repository.AddTaskAsync(task2, CancellationToken.None);

        // Act
        var tasks = await repository.GetTasksAsync(CancellationToken.None);

        // Assert
        tasks.Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(entities)
            ;
    }
}
