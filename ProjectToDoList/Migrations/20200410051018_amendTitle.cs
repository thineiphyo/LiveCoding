using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectToDoList.Migrations
{
    public partial class amendTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ToDoTasks_Title",
                table: "ToDoTasks");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ToDoTasks",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoTasks_Title",
                table: "ToDoTasks",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ToDoTasks_Title",
                table: "ToDoTasks");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ToDoTasks",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoTasks_Title",
                table: "ToDoTasks",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");
        }
    }
}
