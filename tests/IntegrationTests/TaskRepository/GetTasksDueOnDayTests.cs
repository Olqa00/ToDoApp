namespace ToDoApp.IntegrationTests.TaskRepository;

using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Types;
using ToDoApp.Infrastructure.Services;

public sealed class GetTasksDueOnDayTests : IntegrationTestBase
{
    private const string DESCRIPTION_1 = "description-1";
    private const string DESCRIPTION_2 = "description-2";
    private const string DESCRIPTION_3 = "description-2";
    private const string TITLE_1 = "test-task-1";
    private const string TITLE_2 = "test-task-2";
    private const string TITLE_3 = "test-task-2";
    private readonly NullLogger<TaskRepository> logger = new();

    [Fact]
    public async Task Should_ReturnTasksDueOnSpecificDay()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var taskId1 = new TaskId(guid1);
        var taskId2 = new TaskId(guid2);
        var taskId3 = new TaskId(guid3);

        var createdAt = DateTime.UtcNow;
        var dueDate1 = createdAt.AddDays(2).Date;
        var dueDate2 = createdAt.AddDays(3).Date;

        var task1 = new TaskEntity(taskId1, TITLE_1, createdAt, DESCRIPTION_1, dueDate1);
        var task2 = new TaskEntity(taskId2, TITLE_2, createdAt, DESCRIPTION_2, dueDate1);
        var task3 = new TaskEntity(taskId3, TITLE_3, createdAt, DESCRIPTION_3, dueDate2);

        var entitiesDueOnDate = new List<TaskEntity>
        {
            task1,
            task2,
        };

        var repository = new TaskRepository(this.DbContext, this.logger);

        await repository.AddTaskAsync(task1, CancellationToken.None);
        await repository.AddTaskAsync(task2, CancellationToken.None);
        await repository.AddTaskAsync(task3, CancellationToken.None);

        // Act
        var tasksDueOnDate = await repository.GetTasksDueOnDayAsync(dueDate1, CancellationToken.None);

        // Assert
        tasksDueOnDate.Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(entitiesDueOnDate)
            ;
    }
}
