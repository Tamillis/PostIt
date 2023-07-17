using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostItDemo.Migrations
{
    /// <inheritdoc />
    public partial class authorlikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostItId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorLikes_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorLikes_PostIts_PostItId",
                        column: x => x.PostItId,
                        principalTable: "PostIts",
                        principalColumn: "PostItId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorLikes_AuthorId",
                table: "AuthorLikes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorLikes_PostItId",
                table: "AuthorLikes",
                column: "PostItId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorLikes");
        }
    }
}
