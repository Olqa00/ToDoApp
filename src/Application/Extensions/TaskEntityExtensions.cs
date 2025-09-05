namespace ToDoApp.Application.Extensions;

using ToDoApp.Application.Commands;
using ToDoApp.Domain.Entities;

public static class TaskEntityExtensions
{
    public static TaskEntity UpdateFromCommand(this TaskEntity entity, UpdateTask request)
    {
        entity.SetTitle(request.Title);
        entity.SetDescription(request.Description);
        entity.SetPercentComplete(request.PercentComplete, request.CompletedAt);
        entity.Reschedule(request.ExpiryDateTime);

        return entity;
    }
}
