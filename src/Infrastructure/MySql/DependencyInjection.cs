namespace ToDoApp.Infrastructure.MySql;

public static class DependencyInjection
{
    public static IServiceCollection AddMySql(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
