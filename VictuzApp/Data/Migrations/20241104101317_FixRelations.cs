using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VictuzApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipant_Participants_ParticipantsId",
                table: "EventParticipant");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipant",
                table: "EventParticipant");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipant_ParticipantsId",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "ParticipantsId",
                table: "EventParticipant");

            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "EventParticipant",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipant",
                table: "EventParticipant",
                columns: new[] { "EventsId", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipant_UsersId",
                table: "EventParticipant",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipant_AspNetUsers_UsersId",
                table: "EventParticipant",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipant_AspNetUsers_UsersId",
                table: "EventParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipant",
                table: "EventParticipant");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipant_UsersId",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "EventParticipant");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ParticipantsId",
                table: "EventParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipant",
                table: "EventParticipant",
                columns: new[] { "EventsId", "ParticipantsId" });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(251)", maxLength: 251, nullable: false),
                    IsMember = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipant_Participants_ParticipantsId",
                table: "EventParticipant",
                column: "ParticipantsId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
