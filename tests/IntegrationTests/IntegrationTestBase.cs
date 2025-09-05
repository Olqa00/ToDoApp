namespace ToDoApp.IntegrationTests;

using ToDoApp.Infrastructure.MySql;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    private readonly MySqlContainer mySqlContainer;

    internal ToDoDbContext DbContext { get; private set; }

    protected IntegrationTestBase()
    {
        this.mySqlContainer = new MySqlBuilder()
            .WithImage("mysql:8.0")
            .WithDatabase("todo_db")
            .WithUsername("root")
            .WithPassword("P@ssw0rd")
            .Build();
    }

    public async Task DisposeAsync()
    {
        await this.DbContext.DisposeAsync();
        await this.mySqlContainer.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await this.mySqlContainer.StartAsync();

        var options = new DbContextOptionsBuilder<ToDoDbContext>()
            .UseMySql(
                this.mySqlContainer.GetConnectionString(),
                await ServerVersion.AutoDetectAsync(this.mySqlContainer.GetConnectionString()))
            .Options;

        this.DbContext = new ToDoDbContext(options);

        await this.DbContext.Database.EnsureCreatedAsync();
    }
}
