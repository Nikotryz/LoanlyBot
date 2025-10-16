using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LoanlyBot.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_catalog.adminpack", ",,");

            migrationBuilder.CreateTable(
                name: "loaners",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    my_loan = table.Column<decimal>(type: "money", nullable: true),
                    his_loan = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("loaners_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sender_id = table.Column<long>(type: "bigint", nullable: false),
                    recipient_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    amount = table.Column<decimal>(type: "money", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("payments_pkey", x => x.payment_id);
                    table.ForeignKey(
                        name: "payments_recipient_id_fkey",
                        column: x => x.recipient_id,
                        principalTable: "loaners",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "payments_sender_id_fkey",
                        column: x => x.sender_id,
                        principalTable: "loaners",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_recipient_id",
                table: "payments",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_sender_id",
                table: "payments",
                column: "sender_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "loaners");
        }
    }
}
