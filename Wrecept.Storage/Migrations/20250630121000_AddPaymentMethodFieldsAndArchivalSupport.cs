using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wrecept.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethodFieldsAndArchivalSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DueInDays",
                table: "PaymentMethods",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "PaymentMethods",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueInDays",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "PaymentMethods");
        }
    }
}
