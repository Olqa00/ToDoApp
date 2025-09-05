namespace ToDoApp.Application.Commands;

public sealed record class UpdateTask : IRequest
{
    public DateTime? CompletedAt { get; set; }
    public required string Description { get; init; }
    public required DateTime ExpiryDateTime { get; init; }
    public required Guid Id { get; init; }
    public required int PercentComplete { get; init; }
    public required string Title { get; init; }
}
