using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VictuzApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(251)", maxLength: 251, nullable: false),
                    IsMember = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipant",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "int", nullable: false),
                    ParticipantsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipant", x => new { x.EventsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_EventParticipant_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipant_Participants_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Date", "Description", "MaxParticipants", "Title" },
                values: new object[] { 1, new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tijdens de Stickers Maken workshop op Hogeschool Zuyd leren studenten creatieve en praktische ontwerpvaardigheden. Ze ontwerpen hun eigen stickers met grafische software, ontdekken de basisprincipes van kleur, vorm en compositie, en maken vervolgens hun ontwerpen werkelijkheid met printtechnieken en een snijplotter. Deze workshop biedt een leuke, hands-on ervaring waarbij studenten hun creativiteit kunnen uiten en unieke, zelfgemaakte stickers mee naar huis nemen.", 30, "Stickers maken" });

            migrationBuilder.InsertData(
                table: "Participants",
                columns: new[] { "Id", "Discriminator", "Email", "IsMember", "Name" },
                values: new object[,]
                {
                    { 1, "Participant", "Rob.Cilissen@zuyd.nl", true, "Rob Cilissen" },
                    { 2, "BoardMember", "Miel.Noelanders@zuyd.be", true, "Miel Noelanders" },
                    { 3, "Administrator", "admin@admin.nl", true, "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipant_ParticipantsId",
                table: "EventParticipant",
                column: "ParticipantsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventParticipant");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Participants");
        }
    }
}
