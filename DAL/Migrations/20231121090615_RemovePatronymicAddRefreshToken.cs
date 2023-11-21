using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemovePatronymicAddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "patronymic",
                table: "userset");

            migrationBuilder.AddColumn<string>(
                name: "refreshtoken",
                table: "userset",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refreshtoken",
                table: "userset");

            migrationBuilder.AddColumn<string>(
                name: "patronymic",
                table: "userset",
                type: "text",
                nullable: true);
        }
    }
}
