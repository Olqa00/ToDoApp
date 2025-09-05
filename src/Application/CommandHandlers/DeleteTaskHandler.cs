namespace ToDoApp.Application.CommandHandlers;

using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;

internal sealed class DeleteTaskHandler : IRequestHandler<DeleteTask>
{
    private readonly ILogger<DeleteTaskHandler> logger;
    private readonly ITaskRepository repository;

    public DeleteTaskHandler(ILogger<DeleteTaskHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(DeleteTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to delete task");

        var taskId = new TaskId(request.Id);
        var taskEntity = await this.repository.GetTaskByIdAsync(taskId, cancellationToken);

        if (taskEntity is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        await this.repository.DeleteTaskAsync(taskId, cancellationToken);
    }
}
