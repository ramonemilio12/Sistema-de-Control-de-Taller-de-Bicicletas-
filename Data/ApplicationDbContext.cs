using Microsoft.EntityFrameworkCore;
using Sistema_Control_Taller_Bicicletas.Models;

namespace Sistema_Control_Taller_Bicicletas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Bicicleta> Bicicletas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Bicicletas)
                .WithOne(b => b.Cliente)
                .HasForeignKey(b => b.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // --------------------------------------------------
            // SEED DATA: datos iniciales para clientes + bicicletas
            // --------------------------------------------------
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente
                {
                    Id = 1,
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Telefono = "809-555-1001",
                    Email = "juan.perez@example.com",
                    Direccion = "Calle Duarte #12, Santo Domingo",
                    FechaRegistro = new DateTime(2026, 1, 15, 9, 0, 0, DateTimeKind.Local)
                },
                new Cliente
                {
                    Id = 2,
                    Nombre = "María",
                    Apellido = "Gómez",
                    Telefono = "809-555-1002",
                    Email = "maria.gomez@example.com",
                    Direccion = "Av. 27 de Febrero #45, Santiago",
                    FechaRegistro = new DateTime(2026, 2, 10, 10, 30, 0, DateTimeKind.Local)
                },
                new Cliente
                {
                    Id = 3,
                    Nombre = "Carlos",
                    Apellido = "Rodríguez",
                    Telefono = "809-555-1003",
                    Email = "carlos.rodriguez@example.com",
                    Direccion = "Calle San Juan #8, La Vega",
                    FechaRegistro = new DateTime(2026, 3, 5, 14, 0, 0, DateTimeKind.Local)
                }
            );

            modelBuilder.Entity<Bicicleta>().HasData(
                new Bicicleta
                {
                    Id = 1,
                    Marca = "Giant",
                    Modelo = "Talon 3 29",
                    Color = "Negro / Azul",
                    NumeroSerie = "GN2024-000123",
                    Descripcion = "Bicicleta de montaña, uso urbano y fin de semana. Requiere cambio de frenos traseros y lubricación general.",
                    ClienteId = 1,
                    FechaIngreso = new DateTime(2026, 9, 28, 8, 0, 0, DateTimeKind.Local)
                },
                new Bicicleta
                {
                    Id = 2,
                    Marca = "Trek",
                    Modelo = "Dual Sport 2",
                    Color = "Blanco",
                    NumeroSerie = "TRK-DS2-88421",
                    Descripcion = "Híbrida, se trae por ajuste de cambios delanteros y servicio completo (limpieza + lubricación).",
                    ClienteId = 2,
                    FechaIngreso = new DateTime(2026, 10, 1, 9, 15, 0, DateTimeKind.Local)
                },
                new Bicicleta
                {
                    Id = 3,
                    Marca = "Specialized",
                    Modelo = "Allez Sport",
                    Color = "Rojo",
                    NumeroSerie = "SP-ALZ-SP-33190",
                    Descripcion = "Bicicleta de ruta. Ruido en pedalier, se sospecha rodamientos desgastados.",
                    ClienteId = 1,
                    FechaIngreso = new DateTime(2026, 10, 3, 11, 0, 0, DateTimeKind.Local)
                },
                new Bicicleta
                {
                    Id = 4,
                    Marca = "Scott",
                    Modelo = "Aspect 950",
                    Color = "Verde",
                    NumeroSerie = "SC-ASP950-22107",
                    Descripcion = "MTB 29. Revisión general luego de 6 meses sin uso. Cambio de cableado completo.",
                    ClienteId = 3,
                    FechaIngreso = new DateTime(2026, 10, 5, 13, 45, 0, DateTimeKind.Local)
                },
                new Bicicleta
                {
                    Id = 5,
                    Marca = "Merida",
                    Modelo = "Ride 200",
                    Color = "Gris",
                    NumeroSerie = "MR-RD200-55731",
                    Descripcion = "Bicicleta de ciudad del usuario menor. Ajuste de altura de manubrio y asiento, más protectores de plato.",
                    ClienteId = 2,
                    FechaIngreso = new DateTime(2026, 10, 6, 16, 20, 0, DateTimeKind.Local)
                }
            );
        }
    }
}
