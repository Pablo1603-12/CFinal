using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class bancoBBDD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "esqUsuarios");

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "esqUsuarios",
                columns: table => new
                {
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cuenta_confirmada = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fch_expiracion_token = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    fch_registro = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    tlf_movil = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    rol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    nombre_apellidos = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    contraseña = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    foto = table.Column<byte[]>(type: "bytea", nullable: true),
                    token_recuperacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuarios_pkey", x => x.id_usuario);
                });

            migrationBuilder.CreateIndex(
                name: "usuarios_email_key",
                schema: "esqUsuarios",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "esqUsuarios");
        }
    }
}
