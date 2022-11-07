using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PhoneBook.Dal.Enums;

#nullable disable

namespace PhoneBook.Dal.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:gender_type", "none,male,female")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "group",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "phone_category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phone_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    image_base64 = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<GenderType>(type: "gender_type", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    region = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    street = table.Column<string>(type: "text", nullable: true),
                    house = table.Column<int>(type: "integer", nullable: true),
                    block = table.Column<string>(type: "text", nullable: true),
                    flat = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "GroupDbUserDb",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDbUserDb", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupDbUserDb_group_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDbUserDb_user_UsersId",
                        column: x => x.UsersId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phone_data",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    deleted_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phone_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_phone_data_phone_category_category_id",
                        column: x => x.category_id,
                        principalTable: "phone_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_phone_data_user_user_id",
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

            migrationBuilder.CreateIndex(
                name: "IX_group_created_utc",
                table: "group",
                column: "created_utc");

            migrationBuilder.CreateIndex(
                name: "IX_group_deleted_utc",
                table: "group",
                column: "deleted_utc");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDbUserDb_UsersId",
                table: "GroupDbUserDb",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_phone_category_created_utc",
                table: "phone_category",
                column: "created_utc");

            migrationBuilder.CreateIndex(
                name: "IX_phone_category_deleted_utc",
                table: "phone_category",
                column: "deleted_utc");

            migrationBuilder.CreateIndex(
                name: "IX_phone_data_category_id",
                table: "phone_data",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_phone_data_created_utc",
                table: "phone_data",
                column: "created_utc");

            migrationBuilder.CreateIndex(
                name: "IX_phone_data_deleted_utc",
                table: "phone_data",
                column: "deleted_utc");

            migrationBuilder.CreateIndex(
                name: "IX_phone_data_user_id",
                table: "phone_data",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_created_utc",
                table: "user",
                column: "created_utc");

            migrationBuilder.CreateIndex(
                name: "IX_user_deleted_utc",
                table: "user",
                column: "deleted_utc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "GroupDbUserDb");

            migrationBuilder.DropTable(
                name: "phone_data");

            migrationBuilder.DropTable(
                name: "group");

            migrationBuilder.DropTable(
                name: "phone_category");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
