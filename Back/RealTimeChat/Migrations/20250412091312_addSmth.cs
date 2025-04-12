using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimeChat.Migrations
{
    /// <inheritdoc />
    public partial class addSmth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_PrivateChats_PrivateChatId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateChats_Users_UserId",
                table: "PrivateChats");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateChats_Users_UserId1",
                table: "PrivateChats");

            migrationBuilder.DropIndex(
                name: "IX_PrivateChats_UserId",
                table: "PrivateChats");

            migrationBuilder.DropIndex(
                name: "IX_PrivateChats_UserId1",
                table: "PrivateChats");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PrivateChats");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "PrivateChats");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "RoomName",
                table: "PrivateChats",
                newName: "ChatName");

            migrationBuilder.RenameColumn(
                name: "PrivateChatId",
                table: "Messages",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_PrivateChatId",
                table: "Messages",
                newName: "IX_Messages_ChatId");

            migrationBuilder.AddColumn<int>(
                name: "DormitoryId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeenAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Room",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PrivateChats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PrivateChats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_PrivateChats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "PrivateChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_PrivateChats_ChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DormitoryId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastSeenAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PrivateChats");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PrivateChats");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "ChatName",
                table: "PrivateChats",
                newName: "RoomName");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Messages",
                newName: "PrivateChatId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                newName: "IX_Messages_PrivateChatId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PrivateChats",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "PrivateChats",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChats_UserId",
                table: "PrivateChats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChats_UserId1",
                table: "PrivateChats",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_PrivateChats_PrivateChatId",
                table: "Messages",
                column: "PrivateChatId",
                principalTable: "PrivateChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateChats_Users_UserId",
                table: "PrivateChats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateChats_Users_UserId1",
                table: "PrivateChats",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
