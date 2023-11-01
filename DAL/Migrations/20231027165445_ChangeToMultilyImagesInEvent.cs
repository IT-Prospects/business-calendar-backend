using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToMultilyImagesInEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_event_image",
                table: "eventset");

            migrationBuilder.DropIndex(
                name: "IX_eventset_image_id",
                table: "eventset");

            migrationBuilder.DropColumn(
                name: "image_id",
                table: "eventset");

            migrationBuilder.AddColumn<long>(
                name: "EventId",
                table: "imageset",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "event_id",
                table: "imageset",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ismain",
                table: "imageset",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_imageset_event_id",
                table: "imageset",
                column: "event_id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_event_image",
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
                name: "FK_imageset_eventset_EventId",
                table: "imageset");

            migrationBuilder.DropForeignKey(
                name: "fk_event_image",
                table: "imageset");

            migrationBuilder.DropIndex(
                name: "IX_imageset_event_id",
                table: "imageset");

            migrationBuilder.DropIndex(
                name: "IX_imageset_EventId",
                table: "imageset");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "imageset");

            migrationBuilder.DropColumn(
                name: "event_id",
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

            migrationBuilder.CreateIndex(
                name: "IX_eventset_image_id",
                table: "eventset",
                column: "image_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_event_image",
                table: "eventset",
                column: "image_id",
                principalTable: "imageset",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
