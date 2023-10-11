using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddImageAndEditEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "path",
                table: "imageset",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "EventDuration",
                table: "eventset",
                newName: "eventduration");

            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "eventset",
                newName: "eventdate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "eventdate",
                table: "eventset",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_imageset_name",
                table: "imageset",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_imageset_name",
                table: "imageset");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "imageset",
                newName: "path");

            migrationBuilder.RenameColumn(
                name: "eventduration",
                table: "eventset",
                newName: "EventDuration");

            migrationBuilder.RenameColumn(
                name: "eventdate",
                table: "eventset",
                newName: "EventDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "eventset",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
