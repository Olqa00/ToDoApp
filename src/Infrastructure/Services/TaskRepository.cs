namespace ToDoApp.Infrastructure.Services;

using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.Extensions;
using ToDoApp.Infrastructure.Models;
using ToDoApp.Infrastructure.MySql;

internal sealed class TaskRepository : ITaskRepository
{
    private readonly ToDoDbContext dbContext;
    private readonly ILogger<TaskRepository> logger;
    private readonly DbSet<TaskDbModel> tasks;

    public TaskRepository(ToDoDbContext dbContext, ILogger<TaskRepository> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.tasks = dbContext.Tasks;
    }

    public async Task AddTaskAsync(TaskEntity entity, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), entity.Id)
        );

        this.logger.LogInformation("Try to add task to db");

        var dbModel = entity.ToDbModel();

        await this.dbContext.AddAsync(dbModel, cancellationToken);
        await this.dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskEntity?> GetTaskByIdAsync(TaskId id, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(
            (nameof(TaskId), id)
        );

        this.logger.LogInformation("Try to get task from db by id");

        var dbModel = await this.tasks.FirstOrDefaultAsync(task => task.Id == id.Value, cancellationToken);

        if (dbModel is null)
        {
            this.logger.LogWarning("Task not found in db");

            return null;
        }

        var entities = dbModel.ToEntity();

        return entities;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetTasksAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get tasks from db");

        var taskDbModels = await this.tasks.ToListAsync(cancellationToken);

        var entities = taskDbModels.ToEntities();

        return entities;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetTasksDueBetweenAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginScope(new Dictionary<string, object?>
        {
            ["date_from"] = from,
            ["date_to"] = to,
        });

        this.logger.LogInformation("Try to get tasks due between {From} and {To}", from, to);

        var fromDate = from.Date;
        var toDate = to.Date.AddDays(1);

        var taskDbModels = await this.tasks
            .Where(task => task.PercentComplete != 100)
            .Where(task => task.ExpiryDateTime >= fromDate && task.ExpiryDateTime < toDate)
            .ToListAsync(cancellationToken);

        return taskDbModels.ToEntities();
    }

    public async Task<IReadOnlyList<TaskEntity>> GetTasksDueOnDayAsync(DateTime expiryDate, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get tasks by expiry date from db");

        var start = expiryDate.Date;
        var end = start.AddDays(1);

        var taskDbModels = await this.tasks
            .Where(task => task.PercentComplete != 100)
            .Where(task => task.ExpiryDateTime >= start && task.ExpiryDateTime < end)
            .ToListAsync(cancellationToken);

        var entities = taskDbModels.ToEntities();

        return entities;
    }

    public async Task<IReadOnlyList<TaskEntity>> GetUncompletedTasksAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get uncompleted tasks from db");

        var taskDbModels = await this.tasks
            .Where(task => task.PercentComplete != 100)
            .ToListAsync(cancellationToken);

        var entities = taskDbModels.ToEntities();

        return entities;
    }
}
