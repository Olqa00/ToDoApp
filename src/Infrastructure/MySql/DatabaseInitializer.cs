namespace ToDoApp.Infrastructure.MySql;

using Microsoft.Extensions.Hosting;

internal sealed class DatabaseInitializer : IHostedService
{
    private const int MAX_RETRIES = 5;
    private readonly ILogger<DatabaseInitializer> logger;
    private readonly IServiceProvider serviceProvider;

    public DatabaseInitializer(ILogger<DatabaseInitializer> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = this.serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

        var retry = 0;

        while (true)
        {
            try
            {
                this.logger.LogInformation("Applying database migrations...");
                await dbContext.Database.MigrateAsync(cancellationToken);

                break;
            }
            catch (Exception ex) when (retry < MAX_RETRIES)
            {
                retry++;
                var delay = TimeSpan.FromSeconds(5);

                this.logger.LogWarning(ex,
                    "Database migration failed. Retrying in {Delay} seconds... (Attempt {Retry}/{MaxRetries})",
                    delay.TotalSeconds,
                    retry,
                    MAX_RETRIES);

                await Task.Delay(delay, cancellationToken);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
