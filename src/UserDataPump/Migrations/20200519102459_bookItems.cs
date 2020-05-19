using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UserDataPump.Migrations
{
    public partial class bookItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookAccessInfoListPrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<double>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAccessInfoListPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookSearchInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TextSnippet = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSearchInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Epubs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAvailable = table.Column<bool>(nullable: false),
                    AcsTokenLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epubs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SmallThumbnail = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferListPrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AmountInMicros = table.Column<long>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferListPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadingModes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<bool>(nullable: false),
                    Image = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookSaleInfos",
                columns: table => new
                {
                    SaleId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Country = table.Column<string>(nullable: true),
                    Saleability = table.Column<string>(nullable: true),
                    IsEbook = table.Column<bool>(nullable: false),
                    ListPriceId = table.Column<int>(nullable: true),
                    RetailPriceId = table.Column<int>(nullable: true),
                    BuyLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookSaleInfos", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_BookSaleInfos_BookAccessInfoListPrices_ListPriceId",
                        column: x => x.ListPriceId,
                        principalTable: "BookAccessInfoListPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookSaleInfos_BookAccessInfoListPrices_RetailPriceId",
                        column: x => x.RetailPriceId,
                        principalTable: "BookAccessInfoListPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookAccessInfos",
                columns: table => new
                {
                    AccessInfoId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Country = table.Column<string>(nullable: true),
                    Viewability = table.Column<string>(nullable: true),
                    Embeddable = table.Column<bool>(nullable: false),
                    PublicDomain = table.Column<bool>(nullable: false),
                    TextToSpeechPermission = table.Column<string>(nullable: true),
                    EpubId = table.Column<int>(nullable: true),
                    PdfId = table.Column<int>(nullable: true),
                    WebReaderLink = table.Column<string>(nullable: true),
                    AccessViewStatus = table.Column<string>(nullable: true),
                    QuoteSharingAllowed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAccessInfos", x => x.AccessInfoId);
                    table.ForeignKey(
                        name: "FK_BookAccessInfos_Epubs_EpubId",
                        column: x => x.EpubId,
                        principalTable: "Epubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookAccessInfos_Epubs_PdfId",
                        column: x => x.PdfId,
                        principalTable: "Epubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookVolumeInfos",
                columns: table => new
                {
                    Title = table.Column<string>(nullable: false),
                    Authors = table.Column<List<string>>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    PublishedDate = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReadingModesId = table.Column<int>(nullable: true),
                    PageCount = table.Column<long>(nullable: false),
                    PrintType = table.Column<string>(nullable: true),
                    Categories = table.Column<List<string>>(nullable: true),
                    AverageRating = table.Column<double>(nullable: false),
                    RatingsCount = table.Column<long>(nullable: false),
                    MaturityRating = table.Column<string>(nullable: true),
                    AllowAnonLogging = table.Column<bool>(nullable: false),
                    ContentVersion = table.Column<string>(nullable: true),
                    ImageLinksId = table.Column<int>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    PreviewLink = table.Column<string>(nullable: true),
                    InfoLink = table.Column<string>(nullable: true),
                    CanonicalVolumeLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookVolumeInfos", x => x.Title);
                    table.ForeignKey(
                        name: "FK_BookVolumeInfos_ImageLinks_ImageLinksId",
                        column: x => x.ImageLinksId,
                        principalTable: "ImageLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookVolumeInfos_ReadingModes_ReadingModesId",
                        column: x => x.ReadingModesId,
                        principalTable: "ReadingModes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinskyOfferType = table.Column<long>(nullable: false),
                    ListPriceId = table.Column<int>(nullable: true),
                    RetailPriceId = table.Column<int>(nullable: true),
                    BookSaleInfoSaleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_BookSaleInfos_BookSaleInfoSaleId",
                        column: x => x.BookSaleInfoSaleId,
                        principalTable: "BookSaleInfos",
                        principalColumn: "SaleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_OfferListPrices_ListPriceId",
                        column: x => x.ListPriceId,
                        principalTable: "OfferListPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_OfferListPrices_RetailPriceId",
                        column: x => x.RetailPriceId,
                        principalTable: "OfferListPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Etag = table.Column<string>(nullable: true),
                    Kind = table.Column<string>(nullable: true),
                    SelfLink = table.Column<string>(nullable: true),
                    VolumeInfoTitle = table.Column<string>(nullable: true),
                    SaleInfoSaleId = table.Column<int>(nullable: true),
                    AccessInfoId = table.Column<int>(nullable: true),
                    SearchInfoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookItems_BookAccessInfos_AccessInfoId",
                        column: x => x.AccessInfoId,
                        principalTable: "BookAccessInfos",
                        principalColumn: "AccessInfoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookItems_BookSaleInfos_SaleInfoSaleId",
                        column: x => x.SaleInfoSaleId,
                        principalTable: "BookSaleInfos",
                        principalColumn: "SaleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookItems_BookSearchInfos_SearchInfoId",
                        column: x => x.SearchInfoId,
                        principalTable: "BookSearchInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookItems_BookVolumeInfos_VolumeInfoTitle",
                        column: x => x.VolumeInfoTitle,
                        principalTable: "BookVolumeInfos",
                        principalColumn: "Title",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndustryIdentifiers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    BookVolumeInfoTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryIdentifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndustryIdentifiers_BookVolumeInfos_BookVolumeInfoTitle",
                        column: x => x.BookVolumeInfoTitle,
                        principalTable: "BookVolumeInfos",
                        principalColumn: "Title",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAccessInfos_EpubId",
                table: "BookAccessInfos",
                column: "EpubId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAccessInfos_PdfId",
                table: "BookAccessInfos",
                column: "PdfId");

            migrationBuilder.CreateIndex(
                name: "IX_BookItems_AccessInfoId",
                table: "BookItems",
                column: "AccessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BookItems_SaleInfoSaleId",
                table: "BookItems",
                column: "SaleInfoSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_BookItems_SearchInfoId",
                table: "BookItems",
                column: "SearchInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BookItems_VolumeInfoTitle",
                table: "BookItems",
                column: "VolumeInfoTitle");

            migrationBuilder.CreateIndex(
                name: "IX_BookSaleInfos_ListPriceId",
                table: "BookSaleInfos",
                column: "ListPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_BookSaleInfos_RetailPriceId",
                table: "BookSaleInfos",
                column: "RetailPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_BookVolumeInfos_ImageLinksId",
                table: "BookVolumeInfos",
                column: "ImageLinksId");

            migrationBuilder.CreateIndex(
                name: "IX_BookVolumeInfos_ReadingModesId",
                table: "BookVolumeInfos",
                column: "ReadingModesId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryIdentifiers_BookVolumeInfoTitle",
                table: "IndustryIdentifiers",
                column: "BookVolumeInfoTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_BookSaleInfoSaleId",
                table: "Offers",
                column: "BookSaleInfoSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ListPriceId",
                table: "Offers",
                column: "ListPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_RetailPriceId",
                table: "Offers",
                column: "RetailPriceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookItems");

            migrationBuilder.DropTable(
                name: "IndustryIdentifiers");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "BookAccessInfos");

            migrationBuilder.DropTable(
                name: "BookSearchInfos");

            migrationBuilder.DropTable(
                name: "BookVolumeInfos");

            migrationBuilder.DropTable(
                name: "BookSaleInfos");

            migrationBuilder.DropTable(
                name: "OfferListPrices");

            migrationBuilder.DropTable(
                name: "Epubs");

            migrationBuilder.DropTable(
                name: "ImageLinks");

            migrationBuilder.DropTable(
                name: "ReadingModes");

            migrationBuilder.DropTable(
                name: "BookAccessInfoListPrices");
        }
    }
}
