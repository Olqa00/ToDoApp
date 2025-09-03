namespace ToDoApp.Infrastructure.Services;

using Microsoft.Extensions.Logging;
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

    public Task AddTaskAsync(TaskEntity task, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task<TaskResult?> GetTaskByIdAsync(TaskId id, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public async Task<IReadOnlyList<TaskResult>> GetTasksAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get tasks from db");

        var taskDbModels = await this.tasks.ToListAsync(cancellationToken);

        var results = taskDbModels.ToResults();

        return results;
    }
}
