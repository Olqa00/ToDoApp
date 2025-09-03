namespace ToDoApp.Infrastructure.Extensions;

using ToDoApp.Application.Results;
using ToDoApp.Infrastructure.Models;

public static class TaskDbModelExtensions
{
    public static TaskResult ToResult(this TaskDbModel dbModel)
    {
        var isCompleted = dbModel.PercentComplete is 100;

        var result = new TaskResult
        {
            CompletedAt = dbModel.CompletedAt,
            CreatedAt = dbModel.CreatedAt,
            Description = dbModel.Description,
            ExpiryDateTime = dbModel.ExpiryDateTime,
            Id = dbModel.Id,
            IsCompleted = isCompleted,
            PercentComplete = dbModel.PercentComplete,
            Title = dbModel.Title,
        };

        return result;
    }

    public static List<TaskResult> ToResults(this IReadOnlyList<TaskDbModel> dbModels)
    {
        var results = dbModels.Select(dbModel => dbModel.ToResult()).ToList();

        return results;
    }
}
