using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_imageset_eventset_EventId",
                table: "imageset");

            migrationBuilder.DropIndex(
                name: "IX_imageset_EventId",
                table: "imageset");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "imageset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EventId",
                table: "imageset",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_imageset_EventId",
                table: "imageset",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_imageset_eventset_EventId",
                table: "imageset",
                column: "EventId",
                principalTable: "eventset",
                principalColumn: "id");
        }
    }
}
