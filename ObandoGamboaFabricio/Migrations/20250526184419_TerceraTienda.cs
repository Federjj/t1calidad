using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObandoGamboaFabricio.Migrations
{
    /// <inheritdoc />
    public partial class TerceraTienda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estado",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "ciudad",
                table: "Direcciones");

            migrationBuilder.AddColumn<int>(
                name: "id_proveedor",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_estado",
                table: "Pedidos",
                type: "int",
                maxLength: 20,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_repartidor",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "provincia",
                table: "Direcciones",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "id_ciudad",
                table: "Direcciones",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ciudades",
                columns: table => new
                {
                    id_ciudad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_ciudad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudades", x => x.id_ciudad);
                });

            migrationBuilder.CreateTable(
                name: "EstadosDeCompra",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosDeCompra", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "Provedores",
                columns: table => new
                {
                    id_proveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_proveedor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provedores", x => x.id_proveedor);
                });

            migrationBuilder.CreateTable(
                name: "Repartidor",
                columns: table => new
                {
                    id_repartidor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_repatidor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repartidor", x => x.id_repartidor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_id_proveedor",
                table: "Productos",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_id_estado",
                table: "Pedidos",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_id_repartidor",
                table: "Pedidos",
                column: "id_repartidor");

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_id_ciudad",
                table: "Direcciones",
                column: "id_ciudad");

            migrationBuilder.AddForeignKey(
                name: "FK_Direcciones_Ciudades_id_ciudad",
                table: "Direcciones",
                column: "id_ciudad",
                principalTable: "Ciudades",
                principalColumn: "id_ciudad",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_EstadosDeCompra_id_estado",
                table: "Pedidos",
                column: "id_estado",
                principalTable: "EstadosDeCompra",
                principalColumn: "id_estado",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Repartidor_id_repartidor",
                table: "Pedidos",
                column: "id_repartidor",
                principalTable: "Repartidor",
                principalColumn: "id_repartidor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Provedores_id_proveedor",
                table: "Productos",
                column: "id_proveedor",
                principalTable: "Provedores",
                principalColumn: "id_proveedor",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Direcciones_Ciudades_id_ciudad",
                table: "Direcciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_EstadosDeCompra_id_estado",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Repartidor_id_repartidor",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Provedores_id_proveedor",
                table: "Productos");

            migrationBuilder.DropTable(
                name: "Ciudades");

            migrationBuilder.DropTable(
                name: "EstadosDeCompra");

            migrationBuilder.DropTable(
                name: "Provedores");

            migrationBuilder.DropTable(
                name: "Repartidor");

            migrationBuilder.DropIndex(
                name: "IX_Productos_id_proveedor",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_id_estado",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_id_repartidor",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Direcciones_id_ciudad",
                table: "Direcciones");

            migrationBuilder.DropColumn(
                name: "id_proveedor",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "id_estado",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "id_repartidor",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "id_ciudad",
                table: "Direcciones");

            migrationBuilder.AddColumn<string>(
                name: "estado",
                table: "Pedidos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "provincia",
                table: "Direcciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ciudad",
                table: "Direcciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
