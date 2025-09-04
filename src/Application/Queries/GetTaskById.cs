namespace ToDoApp.Application.Queries;

using ToDoApp.Application.Results;

public sealed record class GetTaskById : IRequest<TaskResult>
{
    public required Guid Id { get; init; }
}
