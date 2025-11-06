using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CongresoApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Twitter = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Ocupacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AvatarUrl = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TerminosAceptados = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Email",
                table: "Participants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_Nombre_Apellidos",
                table: "Participants",
                columns: new[] { "Nombre", "Apellidos" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");
        }
    }
}
