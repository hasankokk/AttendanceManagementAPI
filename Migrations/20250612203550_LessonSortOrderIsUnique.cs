using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class LessonSortOrderIsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SortOrder",
                table: "Lessons",
                column: "SortOrder",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_SortOrder",
                table: "Lessons");
        }
    }
}
