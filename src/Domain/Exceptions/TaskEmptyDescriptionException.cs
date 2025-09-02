namespace ToDoApp.Domain.Exceptions;

internal sealed class TaskEmptyDescriptionException : DomainException
{
    public TaskEmptyDescriptionException(TaskId id)
        : base($"The Task {id.Value} has empty description.")
    {
        this.Id = id.Value;
    }
}
