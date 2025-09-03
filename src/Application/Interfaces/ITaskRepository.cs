namespace ToDoApp.Application.Interfaces;

using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;

public interface ITaskRepository
{
    Task AddTaskAsync(TaskEntity task, CancellationToken cancellationToken);
    Task<TaskResult?> GetTaskByIdAsync(TaskId id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaskResult>> GetTasksAsync(CancellationToken cancellationToken);
}
