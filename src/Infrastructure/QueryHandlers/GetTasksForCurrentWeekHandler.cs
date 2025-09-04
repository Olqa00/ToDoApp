namespace ToDoApp.Infrastructure.QueryHandlers;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Queries;
using ToDoApp.Application.Results;
using ToDoApp.Infrastructure.Extensions;

internal sealed class GetTasksForCurrentWeekHandler : IRequestHandler<GetTasksForCurrentWeek, IReadOnlyList<TaskResult>>
{
    private const string DATE_FROM = "date_from";
    private const string DATE_TO = "date_to";
    private readonly ILogger<GetTasksForCurrentWeekHandler> logger;
    private readonly ITaskRepository repository;
    private readonly TimeProvider timeProvider;

    public GetTasksForCurrentWeekHandler(ILogger<GetTasksForCurrentWeekHandler> logger, ITaskRepository repository, TimeProvider timeProvider)
    {
        this.logger = logger;
        this.repository = repository;
        this.timeProvider = timeProvider;
    }

    public async Task<IReadOnlyList<TaskResult>> Handle(GetTasksForCurrentWeek request, CancellationToken cancellationToken)
    {
        var today = this.timeProvider.GetUtcNow().DateTime;
        var (startOfWeek, endOfWeek) = GetStartAndEndOfWeek(today);

        using var loggerScope = this.logger.BeginScope(new Dictionary<string, object?>
        {
            [DATE_FROM] = startOfWeek.Date,
            [DATE_TO] = endOfWeek.Date,
        });

        this.logger.LogInformation("Try to get tasks for current week.");

        var entities = await this.repository.GetTasksDueBetweenAsync(startOfWeek, endOfWeek, cancellationToken);
        var results = entities.ToResults();

        return results;
    }

    private static (DateTime StartOfWeek, DateTime EndOfWeek) GetStartAndEndOfWeek(DateTime date)
    {
        var dayOfWeek = (int)date.DayOfWeek;
        var startOfWeek = date.AddDays(-dayOfWeek);
        var endOfWeek = startOfWeek.AddDays(6);

        return (startOfWeek, endOfWeek);
    }
}
