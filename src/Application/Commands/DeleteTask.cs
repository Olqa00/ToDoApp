namespace ToDoApp.Application.Commands;

public sealed record class DeleteTask : IRequest
{
    public required Guid Id { get; init; }
}
