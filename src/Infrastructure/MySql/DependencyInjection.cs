namespace ToDoApp.Infrastructure.MySql;

using DotNetEnv;

public static class DependencyInjection
{
    private const string ENV_PATH = "../../infra/.env";
    private const string OPTIONS_SECTION_NAME = "MySql";

    public static IServiceCollection AddMySqlDb(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(OPTIONS_SECTION_NAME);
        services.Configure<MySqlOptions>(section);
        var options = configuration.GetOptions<MySqlOptions>(OPTIONS_SECTION_NAME);

        Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ENV_PATH));

        var envConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(envConnectionString) is false)
        {
            options.ConnectionString = envConnectionString;
        }

        services.AddDbContext<ToDoDbContext>(option => option.UseMySql(options.ConnectionString,
            ServerVersion.AutoDetect(options.ConnectionString),
            b => b.MigrationsAssembly(typeof(ToDoDbContext).Assembly.FullName)));

        services.AddHostedService<DatabaseInitializer>();

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
