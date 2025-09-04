namespace ToDoApp.Infrastructure.Exceptions;

public sealed class TaskNotFoundException : InfrastructureException
{
    public TaskNotFoundException(TaskId id)
        : base($"Task with id {id.Value} not found.")
    {
        this.Id = id.Value;
    }
}
