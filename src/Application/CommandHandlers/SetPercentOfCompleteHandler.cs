namespace ToDoApp.Application.CommandHandlers;

using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;

internal sealed class SetPercentOfCompleteHandler : IRequestHandler<SetPercentOfComplete>
{
    private readonly ILogger<SetPercentOfCompleteHandler> logger;
    private readonly ITaskRepository repository;
    private readonly TimeProvider timeProvider;

    public SetPercentOfCompleteHandler(ILogger<SetPercentOfCompleteHandler> logger, ITaskRepository repository, TimeProvider timeProvider)
    {
        this.logger = logger;
        this.repository = repository;
        this.timeProvider = timeProvider;
    }

    public async Task Handle(SetPercentOfComplete request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to set percent of complete for task");

        var taskId = new TaskId(request.Id);
        var taskEntity = await this.repository.GetTaskByIdAsync(taskId, cancellationToken);

        if (taskEntity is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        var completedAt = request.CompletedAt ?? this.timeProvider.GetUtcNow().DateTime;

        taskEntity.SetPercentComplete(request.PercentOfComplete, completedAt);
        await this.repository.UpdateTaskAsync(taskEntity, cancellationToken);
    }
}
