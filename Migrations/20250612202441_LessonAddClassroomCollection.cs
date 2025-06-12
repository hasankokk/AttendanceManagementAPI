using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class LessonAddClassroomCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Classrooms_ClassroomId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ClassroomId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "Lessons");

            migrationBuilder.CreateTable(
                name: "ClassroomLesson",
                columns: table => new
                {
                    ClassroomsId = table.Column<int>(type: "int", nullable: false),
                    LessonsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomLesson", x => new { x.ClassroomsId, x.LessonsId });
                    table.ForeignKey(
                        name: "FK_ClassroomLesson_Classrooms_ClassroomsId",
                        column: x => x.ClassroomsId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomLesson_Lessons_LessonsId",
                        column: x => x.LessonsId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Name",
                table: "Lessons",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomLesson_LessonsId",
                table: "ClassroomLesson",
                column: "LessonsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomLesson");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_Name",
                table: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                table: "Lessons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ClassroomId",
                table: "Lessons",
                column: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Classrooms_ClassroomId",
                table: "Lessons",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id");
        }
    }
}
