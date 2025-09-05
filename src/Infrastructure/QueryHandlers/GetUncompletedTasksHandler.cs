namespace ToDoApp.Infrastructure.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Infrastructure.Extensions;

internal sealed class GetUncompletedTasksHandler : IRequestHandler<GetUncompletedTasks, IReadOnlyList<TaskResult>>
{
    private readonly ILogger<GetUncompletedTasksHandler> logger;
    private readonly ITaskRepository repository;

    public GetUncompletedTasksHandler(ILogger<GetUncompletedTasksHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task<IReadOnlyList<TaskResult>> Handle(GetUncompletedTasks request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get uncompleted tasks.");

        var tasks = await this.repository.GetUncompletedTasksAsync(cancellationToken);

        var results = tasks.ToResults();

        return results;
    }
}
