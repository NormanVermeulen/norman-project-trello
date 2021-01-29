using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace prid_2021_A08.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collaborations",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    BoardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborations", x => new { x.UserId, x.BoardId });
                });

            migrationBuilder.CreateTable(
                name: "Lists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    BoardId = table.Column<int>(nullable: false),
                    Pos = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    CardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participations", x => new { x.UserId, x.CardId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Pseudo = table.Column<string>(maxLength: 10, nullable: false),
                    Password = table.Column<string>(maxLength: 10, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    CardId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boards_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    AuthorId = table.Column<int>(nullable: false),
                    ListId = table.Column<int>(nullable: false),
                    Pos = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Lists_ListId",
                        column: x => x.ListId,
                        principalTable: "Lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "CardId", "Email", "FirstName", "LastName", "Password", "Pseudo", "Role" },
                values: new object[,]
                {
                    { 1, null, null, "benoit@penelle.be", "Benoît", "Penelle", "ben", "ben", 0 },
                    { 2, null, null, "bruno@lacroix.be", "Bruno", "Lacroix", "bruno", "bruno", 0 },
                    { 3, null, null, "norman@vermeulen.be", "Norman", "Vermeulen", "norman", "norman", 0 },
                    { 4, null, null, "jonathan@deversenne.be", "Jonathan", "Deversenne", "jon", "jon", 0 },
                    { 5, null, null, "arthur@denyse.be", "Arthur", "Denyse", "art", "art", 0 },
                    { 6, null, null, "admin@gros.be", "Al", "Admin", "admin", "admin", 2 }
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name", "OwnerId" },
                values: new object[] { 1, "Board1", 1 });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name", "OwnerId" },
                values: new object[] { 3, "Board3", 5 });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name", "OwnerId" },
                values: new object[] { 2, "Board2", 6 });

            migrationBuilder.InsertData(
                table: "Collaborations",
                columns: new[] { "UserId", "BoardId" },
                values: new object[,]
                {
                    { 4, 1 },
                    { 5, 1 },
                    { 3, 1 },
                    { 2, 2 },
                    { 1, 2 },
                    { 6, 2 }
                });

            migrationBuilder.InsertData(
                table: "Lists",
                columns: new[] { "Id", "BoardId", "Name", "Pos" },
                values: new object[,]
                {
                    { 1, 1, "list1", 0 },
                    { 2, 1, "list2", 1 },
                    { 3, 2, "list3", 0 }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "AuthorId", "ListId", "Name", "Pos" },
                values: new object[,]
                {
                    { 3, 1, 1, "card3", 0 },
                    { 4, 1, 1, "card4", 1 },
                    { 1, 1, 2, "card1", 0 },
                    { 2, 1, 2, "card2", 1 }
                });

            migrationBuilder.InsertData(
                table: "Participations",
                columns: new[] { "UserId", "CardId" },
                values: new object[] { 4, 3 });

            migrationBuilder.InsertData(
                table: "Participations",
                columns: new[] { "UserId", "CardId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "Participations",
                columns: new[] { "UserId", "CardId" },
                values: new object[] { 3, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_Name",
                table: "Boards",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boards_OwnerId",
                table: "Boards",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AuthorId",
                table: "Cards",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ListId",
                table: "Cards",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborations_BoardId",
                table: "Collaborations",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_BoardId",
                table: "Lists",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_CardId",
                table: "Participations",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CardId",
                table: "Users",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Pseudo",
                table: "Users",
                column: "Pseudo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborations_Users_UserId",
                table: "Collaborations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborations_Boards_BoardId",
                table: "Collaborations",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Boards_BoardId",
                table: "Lists",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Users_UserId",
                table: "Participations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participations_Cards_CardId",
                table: "Participations",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cards_CardId",
                table: "Users",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_OwnerId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Users_AuthorId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Collaborations");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Lists");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
