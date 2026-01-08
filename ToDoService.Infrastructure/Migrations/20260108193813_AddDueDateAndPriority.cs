using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDueDateAndPriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "ToDos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "ToDos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ToDos");
        }
    }
}
