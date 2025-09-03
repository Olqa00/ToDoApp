namespace ToDoApp.Infrastructure.MySql;

public static class DependencyInjection
{
    private const string OPTIONS_SECTION_NAME = "MySql";

    public static IServiceCollection AddMySql(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(OPTIONS_SECTION_NAME);
        services.Configure<MySqlOptions>(section);
        var options = configuration.GetOptions<MySqlOptions>(OPTIONS_SECTION_NAME);

        Console.WriteLine($"ConnectionString di: {options.ConnectionString}");

        services.AddDbContext<ToDoDbContext>(option => option.UseMySql(options.ConnectionString,
            ServerVersion.AutoDetect(options.ConnectionString),
            b => b.MigrationsAssembly(typeof(ToDoDbContext).Assembly.FullName)));

        return services;
    }

    private static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetSection(sectionName);
        section.Bind(options);

        return options;
    }
}
