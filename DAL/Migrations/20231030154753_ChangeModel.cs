using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_event_image",
                table: "imageset");

            migrationBuilder.DropColumn(
                name: "ismain",
                table: "imageset");

            migrationBuilder.AddColumn<long>(
                name: "image_id",
                table: "eventset",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "fk_event_image",
                table: "eventset",
                column: "image_id",
                principalTable: "imageset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_image_event",
                table: "imageset",
                column: "event_id",
                principalTable: "eventset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_event_image",
                table: "eventset");

            migrationBuilder.DropForeignKey(
                name: "fk_image_event",
                table: "imageset");

            migrationBuilder.DropIndex(
                name: "IX_eventset_image_id",
                table: "eventset");

            migrationBuilder.DropColumn(
                name: "image_id",
                table: "eventset");

            migrationBuilder.AddColumn<bool>(
                name: "ismain",
                table: "imageset",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "fk_event_image",
                table: "imageset",
                column: "event_id",
                principalTable: "eventset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
