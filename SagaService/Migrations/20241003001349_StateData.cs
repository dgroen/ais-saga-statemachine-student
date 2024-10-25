using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SagaService.Migrations
{
    /// <inheritdoc />
    public partial class StateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentStates");

            migrationBuilder.CreateTable(
                name: "StudentStateData",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentCancelDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    StudentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStateData", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentStateData");

            migrationBuilder.CreateTable(
                name: "StudentStates",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentCancelDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStates", x => x.CorrelationId);
                });
        }
    }
}
