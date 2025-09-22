using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObandoGamboaFabricio.Migrations
{
    /// <inheritdoc />
    public partial class cafearoma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articulos_Proveedores_ProveedorId",
                table: "Articulos");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Articulos_ProveedorId",
                table: "Articulos");

            migrationBuilder.RenameColumn(
                name: "ProveedorId",
                table: "Articulos",
                newName: "Stock");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Pedidos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPedido",
                table: "Pedidos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Pedidos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Articulos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Articulos",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "FechaPedido",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Articulos");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Articulos",
                newName: "ProveedorId");

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.IdProveedor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articulos_ProveedorId",
                table: "Articulos",
                column: "ProveedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articulos_Proveedores_ProveedorId",
                table: "Articulos",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "IdProveedor",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
