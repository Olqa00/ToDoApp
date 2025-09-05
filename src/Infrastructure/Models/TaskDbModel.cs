namespace ToDoApp.Infrastructure.Models;

public sealed class TaskDbModel
{
    public DateTime? CompletedAt { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string Description { get; set; }
    public required DateTime ExpiryDateTime { get; set; }
    public required Guid Id { get; set; }
    public required int PercentComplete { get; set; }
    public required string Title { get; set; }
}
