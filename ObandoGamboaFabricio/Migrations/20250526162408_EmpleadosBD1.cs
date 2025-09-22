using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObandoGamboaFabricio.Migrations
{
    /// <inheritdoc />
    public partial class EmpleadosBD1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personajes");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Correo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApellidoUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FecInicio = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "GETDATE()"),
                    rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    IdLibro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnioPublicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    AutorLibro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneroLibro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreLibro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    pagLibro = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.IdLibro);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescripcionRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NomRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Personajes",
                columns: table => new
                {
                    IdPersonaje = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    ApellidoPersonaje = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstadoPersonaje = table.Column<bool>(type: "bit", nullable: false),
                    FecNac = table.Column<DateOnly>(type: "date", nullable: false),
                    LugNac = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombrePersonaje = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personajes", x => x.IdPersonaje);
                    table.ForeignKey(
                        name: "FK_Personajes_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "IdLibro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personajes_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personajes_LibroId",
                table: "Personajes",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Personajes_RolId",
                table: "Personajes",
                column: "RolId");
        }
    }
}
