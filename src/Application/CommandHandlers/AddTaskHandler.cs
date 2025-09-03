namespace ToDoApp.Application.CommandHandlers;

using Microsoft.Extensions.Logging;
using ToDoApp.Application.Commands;
using ToDoApp.Application.Interfaces;

internal sealed class AddTaskHandler : IRequestHandler<AddTask>
{
    private readonly ILogger<AddTaskHandler> logger;
    private readonly ITaskRepository repository;

    public AddTaskHandler(ILogger<AddTaskHandler> logger, ITaskRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public Task Handle(AddTask request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
