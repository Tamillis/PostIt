using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostItDemo.Migrations
{
    /// <inheritdoc />
    public partial class postitlikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "PostIts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "PostIts");
        }
    }
}
