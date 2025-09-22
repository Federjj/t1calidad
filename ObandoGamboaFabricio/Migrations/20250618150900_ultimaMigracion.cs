using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObandoGamboaFabricio.Migrations
{
    /// <inheritdoc />
    public partial class ultimaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Articulos_ProductoId",
                table: "DetallesPedido");

            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "DetallesPedido",
                newName: "IdArticulo");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesPedido_ProductoId",
                table: "DetallesPedido",
                newName: "IX_DetallesPedido_IdArticulo");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Articulos_IdArticulo",
                table: "DetallesPedido",
                column: "IdArticulo",
                principalTable: "Articulos",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesPedido_Articulos_IdArticulo",
                table: "DetallesPedido");

            migrationBuilder.RenameColumn(
                name: "IdArticulo",
                table: "DetallesPedido",
                newName: "ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesPedido_IdArticulo",
                table: "DetallesPedido",
                newName: "IX_DetallesPedido_ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesPedido_Articulos_ProductoId",
                table: "DetallesPedido",
                column: "ProductoId",
                principalTable: "Articulos",
                principalColumn: "IdArticulo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
