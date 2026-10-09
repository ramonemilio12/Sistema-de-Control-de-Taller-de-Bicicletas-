using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Taller_Bicicletas.Models
{
    // Representa a un cliente (dueño de una o varias bicicletas) que se registra en el taller.
    // La guardo con todas las restricciones directamente aquí mediante DataAnnotations,
    // así no necesito tanta configuración manual en Fluent API.
    public class Cliente
    {
        // Identificador único del cliente; uso [Key] explícitamente para que quede claro
        // que es la PK, aunque por convención EF Core también lo reconocería por el nombre "Id".
        [Key]
        public int Id { get; set; }

        // El nombre es obligatorio y no permito que pasen más de 100 caracteres.
        // Lo inicializo en string.Empty para evitar warnings de "non-nullable".
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        // Mismo criterio que el nombre: obligatorio y con tope razonable.
        [Required(ErrorMessage = "El apellido del cliente es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no debe superar los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        // Teléfono es opcional (hay clientes que prefieren pasar solo WhatsApp/correo),
        // pero si lo escriben, limito su tamaño por seguridad.
        [MaxLength(20, ErrorMessage = "El teléfono no debe superar los 20 caracteres.")]
        public string? Telefono { get; set; }

        // También opcional, pero al usarlo luego para reportes o notificaciones le
        // pongo un límite lógico de 150 caracteres.
        [MaxLength(150, ErrorMessage = "El email no debe superar los 150 caracteres.")]
        public string? Email { get; set; }

        // Dirección física (no se pide para nada crítico, por eso es nullable).
        [MaxLength(250, ErrorMessage = "La dirección no debe superar los 250 caracteres.")]
        public string? Direccion { get; set; }

        // Fecha en que se registró por primera vez. Por defecto toma el momento actual
        // de la máquina donde corre la API, pero en producción se podría normalizar a UTC.
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relación 1 a N: un cliente puede tener MUCHAS bicicletas registradas.
        // La inicializo como una lista vacía para que no me explote en NullReference
        // cuando desde el controller quiera agregar/consultar. El lado "muchos" de la
        // relación lo completa Bicicleta.ClienteId + Bicicleta.Cliente.
        public ICollection<Bicicleta> Bicicletas { get; set; } = new List<Bicicleta>();
    }
}
