namespace ToDoApp.Application.CommandHandlers;

using ToDoApp.Application.Commands;
using ToDoApp.Application.Exceptions;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

internal sealed class AddTaskHandler : IRequestHandler<AddTask>
{
    private readonly ILogger<AddTaskHandler> logger;
    private readonly ITaskRepository repository;
    private readonly TimeProvider timeProvider;

    public AddTaskHandler(ILogger<AddTaskHandler> logger, ITaskRepository repository, TimeProvider timeProvider)
    {
        this.logger = logger;
        this.repository = repository;
        this.timeProvider = timeProvider;
    }

    public async Task Handle(AddTask request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), request.Id)
        );

        this.logger.LogInformation("Try to add task");

        var taskId = new TaskId(request.Id);
        var task = await this.repository.GetTaskByIdAsync(taskId, cancellationToken);

        if (task is not null)
        {
            throw new TaskAlreadyExistsException(taskId);
        }

        var createdAt = this.timeProvider.GetUtcNow().DateTime;

        var entity = new TaskEntity(taskId, request.Title, createdAt, request.Description, request.ExpiryDateTime);

        await this.repository.AddTaskAsync(entity, cancellationToken);
    }
}
