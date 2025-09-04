namespace ToDoApp.Application.Queries;

using ToDoApp.Application.Results;

public sealed class GetTodayTasks : IRequest<IReadOnlyList<TaskResult>>
{
}
