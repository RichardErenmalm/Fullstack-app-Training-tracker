using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutHistoryId",
                table: "ExerciseHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkoutHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkoutId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutHistories_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistories_WorkoutHistoryId",
                table: "ExerciseHistories",
                column: "WorkoutHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutHistories_UserId",
                table: "WorkoutHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutHistories_WorkoutId",
                table: "WorkoutHistories",
                column: "WorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseHistories_WorkoutHistories_WorkoutHistoryId",
                table: "ExerciseHistories",
                column: "WorkoutHistoryId",
                principalTable: "WorkoutHistories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseHistories_WorkoutHistories_WorkoutHistoryId",
                table: "ExerciseHistories");

            migrationBuilder.DropTable(
                name: "WorkoutHistories");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseHistories_WorkoutHistoryId",
                table: "ExerciseHistories");

            migrationBuilder.DropColumn(
                name: "WorkoutHistoryId",
                table: "ExerciseHistories");
        }
    }
}
