namespace ToDoApp.Infrastructure;

using ToDoApp.Infrastructure.MySql;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMySql(configuration);

        return services;
    }
}
