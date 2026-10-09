using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Sistema_Control_Taller_Bicicletas.Data
{
    // Usado por la CLI de EF Core (dotnet ef migrations/database update) cuando
    // no puede construir el IHost automáticamente desde Program.cs.
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = FindProjectBasePath();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        // Busca la carpeta raíz del proyecto (donde está appsettings.json).
        private static string FindProjectBasePath()
        {
            var current = Directory.GetCurrentDirectory();
            if (File.Exists(Path.Combine(current, "appsettings.json")))
            {
                return current;
            }

            var asmLocation = AppContext.BaseDirectory;
            var dir = new DirectoryInfo(asmLocation);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "appsettings.json")))
            {
                dir = dir.Parent;
            }

            return dir != null ? dir.FullName : current;
        }
    }
}
