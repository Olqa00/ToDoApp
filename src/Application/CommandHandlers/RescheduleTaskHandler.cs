namespace ToDoApp.Application.CommandHandlers;

using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;

internal sealed class RescheduleTaskHandler : IRequestHandler<RescheduleTask>
{
    private readonly ILogger<RescheduleTaskHandler> logger;
    private readonly ITaskRepository repository;

    public RescheduleTaskHandler(ILogger<RescheduleTaskHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(RescheduleTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to reschedule task");

        var taskId = new TaskId(request.Id);
        var taskEntity = await this.repository.GetTaskByIdAsync(taskId, cancellationToken);

        if (taskEntity is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        taskEntity.Reschedule(request.ExpiryDateTime);
        await this.repository.UpdateTaskAsync(taskEntity, cancellationToken);
    }
}
