namespace ToDoApp.Infrastructure.MySql;

using DotNetEnv;

internal sealed class ToDoDbContextFactory : IDesignTimeDbContextFactory<ToDoDbContext>
{
    private const string ENV_PATH = "../../infra/.env";
    private const string FILE_PATH = @"../WebApi";
    private const string OPTIONS_CONNECTION_STRING = "ConnectionString";
    private const string OPTIONS_SECTION_NAME = "MySql";

    public ToDoDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), FILE_PATH));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ENV_PATH));

        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")
                               ?? configuration.GetSection(OPTIONS_SECTION_NAME)[OPTIONS_CONNECTION_STRING];

        var optionsBuilder = new DbContextOptionsBuilder<ToDoDbContext>();

        optionsBuilder
            .UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly(typeof(ToDoDbContext).Assembly.FullName))
            .ReplaceService<IMigrationsIdGenerator, SimpleMigrationsIdGenerator>();

        return new ToDoDbContext(optionsBuilder.Options);
    }
}
