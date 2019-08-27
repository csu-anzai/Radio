using Microsoft.EntityFrameworkCore.Migrations;

namespace Radio.Migrations
{
    public partial class Channels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Tracks");

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Discriminator = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelTrack",
                columns: table => new
                {
                    ChannelId = table.Column<string>(nullable: false),
                    TrackId = table.Column<string>(nullable: false),
                    Order = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelTrack", x => new { x.ChannelId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_ChannelTrack_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelTrack_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_Name_Discriminator",
                table: "Channels",
                columns: new[] { "Name", "Discriminator" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelTrack_TrackId",
                table: "ChannelTrack",
                column: "TrackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelTrack");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Tracks",
                nullable: false,
                defaultValue: 0);
        }
    }
}
