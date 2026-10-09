using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Control_Taller_Bicicletas.Models
{
    // Representa una bicicleta que ingresa al taller para servicio/reparación.
    // Cada bicicleta PERTENECE a un solo cliente (aunque en la vida real una bici
    // podría pasar de dueño, por ahora se mantiene la regla 1:N simple).
    public class Bicicleta
    {
        // PK de la tabla Bicicletas, autoincremental en SQL por defecto.
        [Key]
        public int Id { get; set; }

        // Marca comercial (Giant, Trek, Specialized, Scott, Merida, etc.). Es obligatoria.
        [Required(ErrorMessage = "La marca de la bicicleta es obligatoria.")]
        [MaxLength(50, ErrorMessage = "La marca no debe superar los 50 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        // Modelo / referencia interna del fabricante. También obligatorio.
        [Required(ErrorMessage = "El modelo de la bicicleta es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El modelo no debe superar los 50 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        // Color descriptivo (algunas bicicletas son muy similares y el color ayuda a
        // diferenciarlas en el taller). Opcional.
        [MaxLength(20, ErrorMessage = "El color no debe superar los 20 caracteres.")]
        public string? Color { get; set; }

        // Número de serie del cuadro. Importante para garantía y evitar confusiones.
        // No lo hago único por defecto, pero en un escenario real sí lo conviene.
        [MaxLength(50, ErrorMessage = "El número de serie no debe superar los 50 caracteres.")]
        public string? NumeroSerie { get; set; }

        // Descripción general: año, problemas reportados, accesorios incluidos, etc.
        // Se le da un poco más de espacio (500 chars) porque suele ser texto libre.
        [MaxLength(500, ErrorMessage = "La descripción no debe superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        // Fecha y hora en que la bicicleta ingresó al taller. Por defecto "ahora".
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        // Clave foránea hacia Cliente.Id. La anotación [ForeignKey] le dice a EF que
        // esta columna "ClienteId" es la FK de la propiedad de navegación Cliente.
        // Como es int (no int?), una bici NO puede existir sin cliente (NOT NULL en SQL).
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        // Propiedad de navegación EF: me permite "acceder al dueño" desde la bici,
        // por ejemplo en LINQ Include(b => b.Cliente). Es nullable por conveniencia
        // (a veces uno crea la bici sin haberle hecho Include todavía).
        public Cliente? Cliente { get; set; }
    }
}
