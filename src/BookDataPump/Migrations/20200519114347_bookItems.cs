using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookDataPump.Migrations
{
    public partial class bookItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Etag = table.Column<string>(nullable: true),
                    Kind = table.Column<string>(nullable: true),
                    SelfLink = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Authors = table.Column<List<string>>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    PublishedDate = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PageCount = table.Column<long>(nullable: false),
                    PrintType = table.Column<string>(nullable: true),
                    Categories = table.Column<List<string>>(nullable: true),
                    AverageRating = table.Column<double>(nullable: false),
                    RatingsCount = table.Column<long>(nullable: false),
                    MaturityRating = table.Column<string>(nullable: true),
                    AllowAnonLogging = table.Column<bool>(nullable: false),
                    ContentVersion = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    PreviewLink = table.Column<string>(nullable: true),
                    InfoLink = table.Column<string>(nullable: true),
                    CanonicalVolumeLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookItems");
        }
    }
}
