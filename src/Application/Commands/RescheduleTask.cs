namespace ToDoApp.Application.Commands;

public sealed record class RescheduleTask : IRequest
{
    public required DateTime ExpiryDateTime { get; init; }
    public required Guid Id { get; init; }
}
