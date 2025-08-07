using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wrecept.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPricingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatRate",
                table: "Products",
                type: "decimal(5,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalGross",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalNet",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalVat",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalGross",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalNet",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalVat",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatRate",
                table: "InvoiceItems",
                type: "decimal(5,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Date",
                table: "Invoices",
                column: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_Date",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalGross",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalNet",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalVat",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalGross",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "TotalNet",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "TotalVat",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "InvoiceItems");
        }
    }
}
