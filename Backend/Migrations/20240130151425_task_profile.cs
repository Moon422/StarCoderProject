using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class task_profile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Tasks_T",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_T_ProfileId",
                table: "Tasks_T",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_T_Profiles_T_ProfileId",
                table: "Tasks_T",
                column: "ProfileId",
                principalTable: "Profiles_T",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_T_Profiles_T_ProfileId",
                table: "Tasks_T");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_T_ProfileId",
                table: "Tasks_T");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Tasks_T");
        }
    }
}
