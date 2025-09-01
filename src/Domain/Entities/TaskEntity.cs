namespace ToDoApp.Domain.Entities;

using ToDoApp.Domain.Exceptions;

public sealed class TaskEntity
{
    public DateTime? CompletedAt { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public string Description { get; private set; }
    public DateTime ExpiryDateTime { get; private set; }
    public TaskId Id { get; private init; }
    private bool IsCompleted => this.CompletedAt.HasValue;
    public int PercentComplete { get; private set; }
    public string Title { get; private set; }

    public TaskEntity(TaskId id, string title, DateTime createdAt, string description, DateTime expiryDateTime)
    {
        this.Id = id;
        this.CreatedAt = createdAt;
        this.ExpiryDateTime = expiryDateTime;
        this.SetDescription(description);
        this.SetTitle(title);

        this.PercentComplete = 0;
    }

    public void Complete(DateTime completedAt)
    {
        if (this.IsCompleted is true)
        {
            throw new TaskAlreadyCompletedException(this.Id);
        }

        if (this.CreatedAt > completedAt)
        {
            throw new TaskCompletedBeforeCreationException(this.Id);
        }

        this.CompletedAt = completedAt;
    }

    public void Reschedule(DateTime newExpiry)
    {
        if (newExpiry < this.CreatedAt)
        {
            throw new TaskInvalidExpiryDateException(this.Id);
        }

        this.ExpiryDateTime = newExpiry;
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new TaskEmptyDescriptionException(this.Id);
        }

        this.Description = description;
    }

    public void SetPercentComplete(int percent)
    {
        if (percent is < 0 or > 100)
        {
            throw new TaskInvalidPercentException(this.Id, percent);
        }

        this.PercentComplete = percent;

        if (percent == 100)
        {
            this.Complete(DateTime.Now);
        }
    }

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new TaskEmptyTitleException(this.Id);
        }

        this.Title = title;
    }

    public void UnComplete()
    {
        if (this.IsCompleted is false)
        {
            throw new TaskNotCompletedException(this.Id);
        }

        this.CompletedAt = null;
    }
}
