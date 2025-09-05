namespace ToDoApp.Infrastructure.MySql;

using ToDoApp.Infrastructure.Models;
using ToDoApp.Infrastructure.MySql.Configuration;

internal sealed class ToDoDbContext : DbContext
{
    public DbSet<TaskDbModel> Tasks { get; set; }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
    }
}
