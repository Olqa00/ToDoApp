namespace ToDoApp.Application.CommandHandlers;

using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;

internal sealed class CompleteTaskHandler : IRequestHandler<CompleteTask>
{
    private readonly ILogger<CompleteTaskHandler> logger;
    private readonly ITaskRepository repository;
    private readonly TimeProvider timeProvider;

    public CompleteTaskHandler(ILogger<CompleteTaskHandler> logger, ITaskRepository repository, TimeProvider timeProvider)
    {
        this.logger = logger;
        this.repository = repository;
        this.timeProvider = timeProvider;
    }

    public async Task Handle(CompleteTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to complete task");

        var taskId = new TaskId(request.Id);
        var taskEntity = await this.repository.GetTaskByIdAsync(taskId, cancellationToken);

        if (taskEntity is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        var completedAt = request.CompletedAt ?? this.timeProvider.GetUtcNow().DateTime;

        taskEntity.Complete(completedAt);
        await this.repository.UpdateTaskAsync(taskEntity, cancellationToken);
    }
}
