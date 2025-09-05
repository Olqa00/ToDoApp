namespace ToDoApp.Application.Commands;

public sealed record class CompleteTask : IRequest
{
    public DateTime? CompletedAt { get; init; }
    public required Guid Id { get; init; }
}
