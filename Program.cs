using Microsoft.EntityFrameworkCore;
using Sistema_Control_Taller_Bicicletas.Data;

// Punto de entrada del proyecto ASP.NET Core.
// Aquí se hace el famoso "builder pattern": 1) Configurar SERVICIOS, 2) construir la app,
// 3) configurar el MIDDLEWARE PIPELINE, 4) arrancarla con app.Run().
// Este archivo reemplaza el viejo Startup.cs de .NET 5 hacia atrás.

var builder = WebApplication.CreateBuilder(args);

// ==============================================================
// 1) AGREGAR SERVICIOS AL CONTENEDOR DE INYECCIÓN DE DEPENDENCIAS
// ==============================================================

// Agrega los controllers Web API. Esto registra ClientesController y
// BicicletasController, habilitando el model binding, validación, JSON de respuesta, etc.
builder.Services.AddControllers();

// Habilita que Swagger descubra endpoints. Lo necesita Swashbuckle para generar el JSON.
builder.Services.AddEndpointsApiExplorer();

// Registra el generador de Swagger (Swashbuckle). Más adelante en este mismo
// archivo se activa la UI del mismo.
builder.Services.AddSwaggerGen();

// Registra el DbContext de EF Core usando el provider de SQL Server.
// La cadena de conexión sale del archivo appsettings.json (ConnectionStrings -> DefaultConnection).
// Por defecto usa un pool de DbContexts, lo que mejora rendimiento.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Fin de configuración de servicios. Construimos la aplicación.
var app = builder.Build();

// ==============================================================
// 2) CONFIGURACIÓN DEL MIDDLEWARE PIPELINE
// ==============================================================
// El orden AQUÍ IMPORTA MUCHO: cada middleware se ejecuta en el orden en que lo escribo.

// Swagger SOLO en Desarrollo. En producción normalmente lo desactivamos (por seguridad)
// o lo protegemos con auth.
if (app.Environment.IsDevelopment())
{
    // Serve /swagger/v1/swagger.json (documento OpenAPI generado a partir de controllers).
    app.UseSwagger();
    // Serve la UI HTML del Swagger en /swagger/index.html.
    app.UseSwaggerUI();
}

// Redirige automáticamente HTTP a HTTPS. En tu perfil "http" de launchSettings no pasa nada
// porque 5000 es HTTP plano; pero si la llaman por 443/5001 redirige bien.
app.UseHttpsRedirection();

// Middleware de autorización. Aún no tenemos [Authorize] en ningún endpoint (porque
// todavía no agregamos JWT), pero es un buen placeholder para cuando lo activemos.
app.UseAuthorization();

// Mapea TODOS los controllers a sus rutas [Route("api/[controller]")] + verbos HTTP.
// Este es el paso que realmente "engancha" ClientesController y BicicletasController
// para que ASP.NET escuche peticiones.
app.MapControllers();

// Arranca el servidor web. Bloquea el hilo y empieza a escuchar peticiones en los
// puertos definidos por launchSettings.json (http://localhost:5000 para el perfil "http").
app.Run();
