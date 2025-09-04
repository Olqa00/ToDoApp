namespace ToDoApp.Application.Interfaces;

using ToDoApp.Domain.Entities;

public interface ITaskRepository
{
    Task AddTaskAsync(TaskEntity entity, CancellationToken cancellationToken);
    Task<TaskEntity?> GetTaskByIdAsync(TaskId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaskEntity>> GetTasksAsync(CancellationToken cancellationToken);
}
