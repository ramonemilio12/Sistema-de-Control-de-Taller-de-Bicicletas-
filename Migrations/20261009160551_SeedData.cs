using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sistema_Control_Taller_Bicicletas.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Apellido", "Direccion", "Email", "FechaRegistro", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Pérez", "Calle Duarte #12, Santo Domingo", "juan.perez@example.com", new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Local), "Juan", "809-555-1001" },
                    { 2, "Gómez", "Av. 27 de Febrero #45, Santiago", "maria.gomez@example.com", new DateTime(2026, 2, 10, 10, 30, 0, 0, DateTimeKind.Local), "María", "809-555-1002" },
                    { 3, "Rodríguez", "Calle San Juan #8, La Vega", "carlos.rodriguez@example.com", new DateTime(2026, 3, 5, 14, 0, 0, 0, DateTimeKind.Local), "Carlos", "809-555-1003" }
                });

            migrationBuilder.InsertData(
                table: "Bicicletas",
                columns: new[] { "Id", "ClienteId", "Color", "Descripcion", "FechaIngreso", "Marca", "Modelo", "NumeroSerie" },
                values: new object[,]
                {
                    { 1, 1, "Negro / Azul", "Bicicleta de montaña, uso urbano y fin de semana. Requiere cambio de frenos traseros y lubricación general.", new DateTime(2026, 9, 28, 8, 0, 0, 0, DateTimeKind.Local), "Giant", "Talon 3 29", "GN2024-000123" },
                    { 2, 2, "Blanco", "Híbrida, se trae por ajuste de cambios delanteros y servicio completo (limpieza + lubricación).", new DateTime(2026, 10, 1, 9, 15, 0, 0, DateTimeKind.Local), "Trek", "Dual Sport 2", "TRK-DS2-88421" },
                    { 3, 1, "Rojo", "Bicicleta de ruta. Ruido en pedalier, se sospecha rodamientos desgastados.", new DateTime(2026, 10, 3, 11, 0, 0, 0, DateTimeKind.Local), "Specialized", "Allez Sport", "SP-ALZ-SP-33190" },
                    { 4, 3, "Verde", "MTB 29. Revisión general luego de 6 meses sin uso. Cambio de cableado completo.", new DateTime(2026, 10, 5, 13, 45, 0, 0, DateTimeKind.Local), "Scott", "Aspect 950", "SC-ASP950-22107" },
                    { 5, 2, "Gris", "Bicicleta de ciudad del usuario menor. Ajuste de altura de manubrio y asiento, más protectores de plato.", new DateTime(2026, 10, 6, 16, 20, 0, 0, DateTimeKind.Local), "Merida", "Ride 200", "MR-RD200-55731" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bicicletas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bicicletas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bicicletas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bicicletas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bicicletas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
