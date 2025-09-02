namespace ToDoApp.Infrastructure.MySql.Configuration;

using ToDoApp.Infrastructure.Models;

internal sealed class TaskConfiguration : IEntityTypeConfiguration<TaskDbModel>
{
    public void Configure(EntityTypeBuilder<TaskDbModel> builder)
    {
        builder.HasKey(task => task.Id);
    }
}
