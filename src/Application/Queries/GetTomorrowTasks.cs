namespace ToDoApp.Application.Queries;

using ToDoApp.Application.Results;

public sealed class GetTomorrowTasks : IRequest<IReadOnlyList<TaskResult>>
{
}
