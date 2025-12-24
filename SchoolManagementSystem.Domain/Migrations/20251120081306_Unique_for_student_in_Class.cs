using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Unique_for_student_in_Class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentClasses_StudentId",
                table: "StudentClasses");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_StudentId_ClassBatchId",
                table: "StudentClasses",
                columns: new[] { "StudentId", "ClassBatchId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentClasses_StudentId_ClassBatchId",
                table: "StudentClasses");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_StudentId",
                table: "StudentClasses",
                column: "StudentId");
        }
    }
}
