namespace ToDoApp.Application.CommandHandlers;

using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Extensions;
using ToDoApp.Application.Interfaces;

internal sealed class UpdateTaskHandler : IRequestHandler<UpdateTask>
{
    private readonly ILogger<UpdateTaskHandler> logger;
    private readonly ITaskRepository repository;

    public UpdateTaskHandler(ILogger<UpdateTaskHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(UpdateTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to update task");

        var taskId = new TaskId(request.Id);
        var taskEntity = await this.repository.GetTaskByIdAsync(taskId, cancellationToken);

        if (taskEntity is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        taskEntity.UpdateFromCommand(request);

        await this.repository.UpdateTaskAsync(taskEntity, cancellationToken);
    }
}
