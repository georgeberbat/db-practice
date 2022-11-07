using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhoneBook.Dal.Migrations
{
    public partial class RemoveUnusedModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    block = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    created_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    flat = table.Column<int>(type: "integer", nullable: true),
                    house = table.Column<int>(type: "integer", nullable: true),
                    region = table.Column<string>(type: "text", nullable: true),
                    street = table.Column<string>(type: "text", nullable: true),
                    updated_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_created_utc",
                table: "address",
                column: "created_utc");

            migrationBuilder.CreateIndex(
                name: "IX_address_user_id",
                table: "address",
                column: "user_id");
        }
    }
}
