namespace ToDoApp.Application.Queries;

using ToDoApp.Application.Results;

public sealed class GetTasksForCurrentWeek : IRequest<IReadOnlyList<TaskResult>>
{
}
