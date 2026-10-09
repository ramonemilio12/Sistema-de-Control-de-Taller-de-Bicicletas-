using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    // DTO de ESCRITURA (entrada de la API) para CREAR un cliente nuevo.
    // Noten que NO tiene Id ni FechaRegistro: son datos que el servidor calcula,
    // no datos que el cliente envía. Así evito que un "usuario malo" trate de
    // poner un Id falso o una fecha de registro manual.
    //
    // Si en el futuro quiero edición, crearía un "ActualizarClienteDto" casi igual,
    // pero con el Id obligatorio.
    public class CrearClienteDto
    {
        // El nombre es obligatorio, 100 chars máximo.
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        // Apellido igual que el nombre: obligatorio y limitado.
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no debe exceder los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        // Teléfono opcional pero corto.
        [MaxLength(20, ErrorMessage = "El teléfono no debe exceder los 20 caracteres.")]
        public string? Telefono { get; set; }

        // Correo opcional, PERO si se envía debe tener formato de email (gracias a
        // [EmailAddress], ASP.NET lo valida automáticamente).
        [MaxLength(150, ErrorMessage = "El email no debe exceder los 150 caracteres.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public string? Email { get; set; }

        // Dirección opcional.
        [MaxLength(250, ErrorMessage = "La dirección no debe exceder los 250 caracteres.")]
        public string? Direccion { get; set; }
    }
}
