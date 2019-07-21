using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceURL.Migrations
{
    public partial class mymigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Link",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    LongUrl = table.Column<string>(nullable: true),
                    ShotUrl = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    NumberOfTransitions = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Link", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Link");
        }
    }
}
