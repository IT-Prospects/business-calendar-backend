using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNmae : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "imageset",
                newName: "url");

            migrationBuilder.RenameIndex(
                name: "IX_imageset_name",
                table: "imageset",
                newName: "IX_imageset_url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "url",
                table: "imageset",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "IX_imageset_url",
                table: "imageset",
                newName: "IX_imageset_name");
        }
    }
}
