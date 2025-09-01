namespace ToDoApp.Domain.Exceptions;

internal sealed class TaskInvalidPercentException : DomainException
{
    public TaskInvalidPercentException(TaskId id, int invalidPercent)
        : base($"Task with id '{id.Value}' cannot have percent complete set to {invalidPercent}. Allowed range is 0–100.")
    {
        this.Id = id.Value;
    }
}
