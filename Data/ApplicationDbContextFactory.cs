using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Sistema_Control_Taller_Bicicletas.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = FindProjectBasePath();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        private static string FindProjectBasePath()
        {
            // Primero probamos con el directorio actual (donde el usuario ejecuta dotnet ef)
            var current = Directory.GetCurrentDirectory();
            if (File.Exists(Path.Combine(current, "appsettings.json")))
            {
                return current;
            }

            // Fallback: buscamos hacia arriba desde la ubicaci�n del ensamblado
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
