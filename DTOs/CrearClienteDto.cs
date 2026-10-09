using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    // Cuerpo de la petición para crear un cliente.
    public class CrearClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no debe exceder los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "El teléfono no debe exceder los 20 caracteres.")]
        public string? Telefono { get; set; }

        [MaxLength(150, ErrorMessage = "El email no debe exceder los 150 caracteres.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public string? Email { get; set; }

        [MaxLength(250, ErrorMessage = "La dirección no debe exceder los 250 caracteres.")]
        public string? Direccion { get; set; }
    }
}
