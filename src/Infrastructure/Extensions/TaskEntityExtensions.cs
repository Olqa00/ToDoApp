namespace ToDoApp.Infrastructure.Extensions;

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
}
