﻿namespace ToDoApp.Infrastructure;

using System.Reflection;
using ToDoApp.Application.Interfaces;
using ToDoApp.Infrastructure.MySql;
using ToDoApp.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddMySql(configuration);

        services.AddScoped<ITaskRepository, TaskRepository>();

        return services;
    }
}
