namespace ToDoApp.Infrastructure.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Infrastructure.Extensions;

internal sealed class GetTomorrowTasksHandler : IRequestHandler<GetTomorrowTasks, IReadOnlyList<TaskResult>>
{
    private const string DATE = "date";
    private readonly ILogger<GetTomorrowTasksHandler> logger;
    private readonly ITaskRepository repository;
    private readonly TimeProvider timeProvider;

    public GetTomorrowTasksHandler(ILogger<GetTomorrowTasksHandler> logger, ITaskRepository repository, TimeProvider timeProvider)
    {
        this.logger = logger;
        this.repository = repository;
        this.timeProvider = timeProvider;
    }

    public async Task<IReadOnlyList<TaskResult>> Handle(GetTomorrowTasks request, CancellationToken cancellationToken)
    {
        var tomorrow = this.timeProvider.GetUtcNow().AddDays(1).DateTime;

        using var loggerScope = this.logger.BeginScope(
            (DATE, tomorrow.Date)
        );

        this.logger.LogInformation("Try to get tomorrow tasks.");

        var entities = await this.repository.GetTasksDueOnDayAsync(tomorrow, cancellationToken);

        var results = entities.ToResults();

        return results;
    }
}
