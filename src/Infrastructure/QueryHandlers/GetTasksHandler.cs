namespace ToDoApp.Infrastructure.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Infrastructure.Extensions;

public sealed class GetTasksHandler : IRequestHandler<GetTasks, IReadOnlyList<TaskResult>>
{
    private readonly ILogger<GetTasksHandler> logger;
    private readonly ITaskRepository repository;

    public GetTasksHandler(ILogger<GetTasksHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task<IReadOnlyList<TaskResult>> Handle(GetTasks request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get tasks.");

        var tasks = await this.repository.GetTasksAsync(cancellationToken);

        var results = tasks.ToResults();

        return results;
    }
}
