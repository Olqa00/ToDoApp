namespace ToDoApp.Infrastructure.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Infrastructure.Extensions;

internal sealed class GetTaskByIdHandler : IRequestHandler<GetTaskById, TaskResult>
{
    private readonly ILogger<GetTaskByIdHandler> logger;
    private readonly ITaskRepository repository;

    public GetTaskByIdHandler(ILogger<GetTaskByIdHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task<TaskResult> Handle(GetTaskById request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to get task by id.");

        var taskId = new TaskId(request.Id);
        var entity = await this.repository.GetTaskByIdAsync(taskId, cancellationToken);

        var result = entity?.ToResult();

        return result;
    }
}
