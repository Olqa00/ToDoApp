namespace ToDoApp.Infrastructure.Extensions;

using ToDoApp.Application.Results;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.Models;

public static class TaskEntityExtensions
{
    public static TaskDbModel ToDbModel(this TaskEntity entity)
    {
        var dbModel = new TaskDbModel
        {
            CompletedAt = entity.CompletedAt,
            CreatedAt = entity.CreatedAt,
            Description = entity.Description,
            ExpiryDateTime = entity.ExpiryDateTime,
            Id = entity.Id.Value,
            PercentComplete = entity.PercentComplete,
            Title = entity.Title,
        };

        return dbModel;
    }

    public static TaskResult ToResult(this TaskEntity entity)
    {
        var isCompleted = entity.PercentComplete is 100;

        var result = new TaskResult
        {
            CompletedAt = entity.CompletedAt,
            CreatedAt = entity.CreatedAt,
            Description = entity.Description,
            ExpiryDateTime = entity.ExpiryDateTime,
            Id = entity.Id.Value,
            IsCompleted = isCompleted,
            PercentComplete = entity.PercentComplete,
            Title = entity.Title,
        };

        return result;
    }

    public static List<TaskResult> ToResults(this IReadOnlyList<TaskEntity> entities)
    {
        var results = entities.Select(entity => entity.ToResult()).ToList();

        return results;
    }
}
