using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MessangerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    MessageStatus = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("status_pkey", x => x.MessageStatus);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    to_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    MessageStatus = table.Column<int>(type: "integer", nullable: false),
                    StatusMessageStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("message_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_Statuses_StatusMessageStatus",
                        column: x => x.StatusMessageStatus,
                        principalTable: "Statuses",
                        principalColumn: "MessageStatus",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "MessageStatus", "Name" },
                values: new object[,]
                {
                    { 0, "NotDelivered" },
                    { 1, "Delivered" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_messages_StatusMessageStatus",
                table: "messages",
                column: "StatusMessageStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
