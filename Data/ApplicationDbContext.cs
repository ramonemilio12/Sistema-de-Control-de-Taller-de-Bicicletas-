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
        }
    }
}
