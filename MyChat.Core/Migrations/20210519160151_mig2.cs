using Microsoft.EntityFrameworkCore.Migrations;

namespace MyChat.Core.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDate",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDate",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
