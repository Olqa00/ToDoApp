namespace ToDoApp.Infrastructure;

using ToDoApp.Application.Interfaces;
using ToDoApp.Infrastructure.MySql;
using ToDoApp.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMySql(configuration);

        services.AddScoped<ITaskRepository, TaskRepository>();

        return services;
    }
}
