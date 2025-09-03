#nullable disable

namespace ToDoApp.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Tasks");
    }

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "Tasks",
                table => new
                {
                    Id = table.Column<Guid>("char(36)", nullable: false, collation: "ascii_general_ci"),
                    CompletedAt = table.Column<DateTime>("datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>("datetime(6)", nullable: false),
                    Description = table.Column<string>("longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryDateTime = table.Column<DateTime>("datetime(6)", nullable: false),
                    PercentComplete = table.Column<int>("int", nullable: false),
                    Title = table.Column<string>("longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                })
            .Annotation("MySql:CharSet", "utf8mb4");
    }
}
