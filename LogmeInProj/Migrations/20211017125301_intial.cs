using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace LogmeInProj.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    DoB = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustId);
                });

            migrationBuilder.CreateTable(
                name: "Creditcard",
                columns: table => new
                {
                    CreditCardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    CardType = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<string>(type: "text", nullable: true),
                    CVV = table.Column<string>(type: "text", nullable: true),
                    CustomerCustId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creditcard", x => x.CreditCardID);
                    table.ForeignKey(
                        name: "FK_Creditcard_Customer_CustomerCustId",
                        column: x => x.CustomerCustId,
                        principalTable: "Customer",
                        principalColumn: "CustId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Creditcard_CustomerCustId",
                table: "Creditcard",
                column: "CustomerCustId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Creditcard");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
