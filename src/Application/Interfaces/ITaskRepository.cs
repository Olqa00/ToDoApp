namespace ToDoApp.Application.Interfaces;

using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;

public interface ITaskRepository
{
    Task AddTaskAsync(TaskEntity entity, CancellationToken cancellationToken);
    Task<TaskResult?> GetTaskByIdAsync(TaskId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaskResult>> GetTasksAsync(CancellationToken cancellationToken);
}
