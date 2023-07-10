using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostItDemo.Migrations
{
    /// <inheritdoc />
    public partial class addedmotherposts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostIts_Authors_AuthorId",
                table: "PostIts");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "PostIts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MotherPostIt",
                table: "PostIts",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostIts_Authors_AuthorId",
                table: "PostIts",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostIts_Authors_AuthorId",
                table: "PostIts");

            migrationBuilder.DropColumn(
                name: "MotherPostIt",
                table: "PostIts");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "PostIts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostIts_Authors_AuthorId",
                table: "PostIts",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
