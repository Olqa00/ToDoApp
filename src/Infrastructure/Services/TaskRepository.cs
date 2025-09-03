namespace ToDoApp.Infrastructure.Services;

using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Results;
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

    public async Task<TaskResult?> GetTaskByIdAsync(TaskId id, CancellationToken cancellationToken)
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

        var result = dbModel.ToResult();

        return result;
    }

    public async Task<IReadOnlyList<TaskResult>> GetTasksAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get tasks from db");

        var taskDbModels = await this.tasks.ToListAsync(cancellationToken);

        var results = taskDbModels.ToResults();

        return results;
    }
}
