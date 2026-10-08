using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    public class CrearClienteDto
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [MaxLength(150)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(250)]
        public string? Direccion { get; set; }
    }
}
