using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Sistema_Control_Taller_Bicicletas.Data
{
    // FÁBRICA en TIEMPO DE DISEÑO (Design Time).
    // ¿Por qué existe? Porque la CLI de EF Core ("dotnet ef migrations add X" o
    // "dotnet ef database update") a veces NO logra construir el IHost de ASP.NET
    // (por ejemplo, cuando appsettings.json tiene un error de formato, o porque
    // la tool corre desde la carpeta "bin" y no encuentra los archivos).
    //
    // En ese escenario, EF se fija: "¿existe alguna clase que implemente
    // IDesignTimeDbContextFactory<MiDbContext>?" Y si la encuentra, LA USA.
    //
    // De ahí que esta clase sea un "fix de emergencia" muy recomendado.
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        // Método que EF llama para construir el DbContext sin depender de Program.cs.
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Primero averiguo en qué carpeta del proyecto está el appsettings.json,
            // porque la CLI puede ejecutarse desde "bin/Debug/net8.0" y cagarla.
            var basePath = FindProjectBasePath();

            // Cargo la configuración exactamente igual que Program.cs:
            //   1- appsettings.json (obligatorio)
            //   2- appsettings.Development.json (opcional, sobreescribe cosas si existe)
            //   3- Variables de entorno (útil en Azure/AppService)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Armo las opciones del DbContext con SQL Server y la cadena que acabamos de leer.
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            // Devuelvo el DbContext listo para que la CLI de EF lo use en migraciones/update.
            return new ApplicationDbContext(optionsBuilder.Options);
        }

        // Método auxiliar para localizar el directorio RAÍZ del proyecto.
        // La idea es: "encuentra la carpeta donde viva appsettings.json y retórnala".
        private static string FindProjectBasePath()
        {
            // Primer intento: la carpeta desde la que el usuario ejecutó "dotnet ef ...".
            // Si el comando lo corre desde el .csproj, aquí ya lo encontramos.
            var current = Directory.GetCurrentDirectory();
            if (File.Exists(Path.Combine(current, "appsettings.json")))
            {
                return current;
            }

            // Plan B: subo niveles desde la ubicación real del .dll compilado
            // (AppContext.BaseDirectory normalmente es bin/Debug/net8.0).
            var asmLocation = AppContext.BaseDirectory;
            var dir = new DirectoryInfo(asmLocation);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "appsettings.json")))
            {
                dir = dir.Parent;
            }

            // Si lo encontramos en alguna subida, lo usamos; si no, devolvemos
            // el directorio actual para que al menos el error sea claro.
            return dir != null ? dir.FullName : current;
        }
    }
}
