using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutExerciseIdToExerciseHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutExerciseId",
                table: "ExerciseHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistories_WorkoutExerciseId",
                table: "ExerciseHistories",
                column: "WorkoutExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseHistories_WorkoutExercises_WorkoutExerciseId",
                table: "ExerciseHistories",
                column: "WorkoutExerciseId",
                principalTable: "WorkoutExercises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseHistories_WorkoutExercises_WorkoutExerciseId",
                table: "ExerciseHistories");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseHistories_WorkoutExerciseId",
                table: "ExerciseHistories");

            migrationBuilder.DropColumn(
                name: "WorkoutExerciseId",
                table: "ExerciseHistories");
        }
    }
}
