using Microsoft.EntityFrameworkCore.Migrations;

namespace Radio.Migrations
{
    public partial class ChannelOwners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Channels",
                nullable: false,
                defaultValue: "ba15be3a-68a1-4a99-9eb6-f3262658042b"); // Requires default owner ID - use admin's ID to give ownership of all channels

            migrationBuilder.CreateIndex(
                name: "IX_Channels_OwnerId",
                table: "Channels",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_AspNetUsers_OwnerId",
                table: "Channels",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AspNetUsers_OwnerId",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Channels_OwnerId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Channels");
        }
    }
}
