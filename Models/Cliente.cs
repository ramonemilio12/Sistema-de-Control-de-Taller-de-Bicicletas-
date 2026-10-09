using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Taller_Bicicletas.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido del cliente es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no debe superar los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "El teléfono no debe superar los 20 caracteres.")]
        public string? Telefono { get; set; }

        [MaxLength(150, ErrorMessage = "El email no debe superar los 150 caracteres.")]
        public string? Email { get; set; }

        [MaxLength(250, ErrorMessage = "La dirección no debe superar los 250 caracteres.")]
        public string? Direccion { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relación 1:N con Bicicletas.
        public ICollection<Bicicleta> Bicicletas { get; set; } = new List<Bicicleta>();
    }
}
