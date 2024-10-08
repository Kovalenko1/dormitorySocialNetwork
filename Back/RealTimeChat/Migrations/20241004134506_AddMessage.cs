using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RealTimeChat.Migrations
{
    /// <inheritdoc />
    public partial class AddMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatParticipants_GroupChats_GroupChatId",
                table: "GroupChatParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatParticipants_Users_UserId",
                table: "GroupChatParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChats",
                table: "GroupChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChatParticipants",
                table: "GroupChatParticipants");

            migrationBuilder.RenameTable(
                name: "GroupChats",
                newName: "GroupChat");

            migrationBuilder.RenameTable(
                name: "GroupChatParticipants",
                newName: "GroupChatParticipant");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatParticipants_UserId",
                table: "GroupChatParticipant",
                newName: "IX_GroupChatParticipant_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GroupChatParticipant",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChat",
                table: "GroupChat",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChatParticipant",
                table: "GroupChatParticipant",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    File = table.Column<List<string>>(type: "text[]", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PrivateChatId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_PrivateChats_PrivateChatId",
                        column: x => x.PrivateChatId,
                        principalTable: "PrivateChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatParticipant_GroupChatId",
                table: "GroupChatParticipant",
                column: "GroupChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PrivateChatId",
                table: "Messages",
                column: "PrivateChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatParticipant_GroupChat_GroupChatId",
                table: "GroupChatParticipant",
                column: "GroupChatId",
                principalTable: "GroupChat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatParticipant_Users_UserId",
                table: "GroupChatParticipant",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatParticipant_GroupChat_GroupChatId",
                table: "GroupChatParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatParticipant_Users_UserId",
                table: "GroupChatParticipant");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChatParticipant",
                table: "GroupChatParticipant");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatParticipant_GroupChatId",
                table: "GroupChatParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupChat",
                table: "GroupChat");

            migrationBuilder.RenameTable(
                name: "GroupChatParticipant",
                newName: "GroupChatParticipants");

            migrationBuilder.RenameTable(
                name: "GroupChat",
                newName: "GroupChats");

            migrationBuilder.RenameIndex(
                name: "IX_GroupChatParticipant_UserId",
                table: "GroupChatParticipants",
                newName: "IX_GroupChatParticipants_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GroupChatParticipants",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChatParticipants",
                table: "GroupChatParticipants",
                columns: new[] { "GroupChatId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupChats",
                table: "GroupChats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatParticipants_GroupChats_GroupChatId",
                table: "GroupChatParticipants",
                column: "GroupChatId",
                principalTable: "GroupChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatParticipants_Users_UserId",
                table: "GroupChatParticipants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
