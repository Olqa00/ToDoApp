namespace ToDoApp.Infrastructure.MySql;

using Microsoft.EntityFrameworkCore.Design;

internal sealed class ToDoDbContextFactory : IDesignTimeDbContextFactory<ToDoDbContext>
{
    public ToDoDbContext CreateDbContext(string[] args)
        => throw new NotImplementedException();
}
