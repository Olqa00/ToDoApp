namespace ToDoApp.Infrastructure.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Infrastructure.Extensions;

internal sealed class GetTodayTasksHandler : IRequestHandler<GetTodayTasks, IReadOnlyList<TaskResult>>
{
    private const string DATE = "date";
    private readonly ILogger<GetTodayTasksHandler> logger;
    private readonly ITaskRepository repository;
    private readonly TimeProvider timeProvider;

    public GetTodayTasksHandler(ILogger<GetTodayTasksHandler> logger, ITaskRepository repository, TimeProvider timeProvider)
    {
        this.logger = logger;
        this.repository = repository;
        this.timeProvider = timeProvider;
    }

    public async Task<IReadOnlyList<TaskResult>> Handle(GetTodayTasks request, CancellationToken cancellationToken)
    {
        var today = this.timeProvider.GetUtcNow().DateTime;

        using var loggerScope = this.logger.BeginScope(
            (DATE, today.Date)
        );

        this.logger.LogInformation("Try to get today tasks.");

        var entities = await this.repository.GetTasksDueOnDay(today, cancellationToken);

        var results = entities.ToResults();

        return results;
    }
}
