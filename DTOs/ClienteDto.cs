namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    // DTO de LECTURA para un cliente. Se usa cuando la API LE DEVUELVE datos al
    // frontend/Swagger. Incluye el Id (obligatorio para mostrar/editar) y los
    // campos visibles del cliente.
    //
    // ¿Por qué no uso directamente la entidad Cliente?
    //   1. En el futuro puedo querer ocultar campos de la entidad (ej: un id interno
    //      secreto) sin romper el response.
    //   2. Puedo calcular campos "de vuelta" sin meterlos en la tabla SQL (aún aquí
    //      no lo hago, pero para ClienteDto es un buen sitio para FullName).
    public class ClienteDto
    {
        // PK del cliente (generada por SQL).
        public int Id { get; set; }

        // Nombre real del cliente.
        public string Nombre { get; set; } = string.Empty;

        // Apellido(s) del cliente.
        public string Apellido { get; set; } = string.Empty;

        // Teléfono (puede ser nulo).
        public string? Telefono { get; set; }

        // Correo (puede ser nulo).
        public string? Email { get; set; }

        // Dirección física (puede ser nula).
        public string? Direccion { get; set; }

        // Fecha en que se dio de alta.
        public DateTime FechaRegistro { get; set; }
    }
}
