namespace ToDoApp.Application.Queries;

using ToDoApp.Application.Results;

public sealed class GetUncompletedTasks : IRequest<IReadOnlyList<TaskResult>>
{
}
