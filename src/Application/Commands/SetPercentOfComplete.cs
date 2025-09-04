namespace ToDoApp.Application.Commands;

public sealed record class SetPercentOfComplete : IRequest
{
    public DateTime? CompletedAt { get; init; }
    public required Guid Id { get; init; }
    public required int PercentOfComplete { get; init; }
}
