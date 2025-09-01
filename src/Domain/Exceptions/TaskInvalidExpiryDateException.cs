namespace ToDoApp.Domain.Exceptions;

internal sealed class TaskInvalidExpiryDateException : DomainException
{
    public TaskInvalidExpiryDateException(TaskId id)
        : base($"Task with id '{id.Value}' cannot have due date earlier than its creation date.")
    {
        this.Id = id.Value;
    }
}
