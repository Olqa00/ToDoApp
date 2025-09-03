namespace ToDoApp.Infrastructure.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;

internal sealed class GetTasksHandler : IRequestHandler<GetTasks, IReadOnlyList<TaskResult>>
{
    private readonly ILogger<GetTasksHandler> logger;
    private readonly ITaskRepository taskRepository;

    public GetTasksHandler(ILogger<GetTasksHandler> logger, ITaskRepository taskRepository)
    {
        this.logger = logger;
        this.taskRepository = taskRepository;
    }

    public async Task<IReadOnlyList<TaskResult>> Handle(GetTasks request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get tasks.");

        var tasks = await this.taskRepository.GetTasksAsync(cancellationToken);

        return tasks;
    }
}
