namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    // DTO de LECTURA para bicicletas. Se usa cuando la API responde con info de una
    // bicicleta. A diferencia de la entidad Bicicleta, aquí ya llego con el nombre
    // completo del dueño (ClienteNombre), porque al frontend normalmente le sirve
    // mostrar "Juan Pérez" en vez de "ClienteId: 1".
    public class BicicletaDto
    {
        // PK de la bicicleta.
        public int Id { get; set; }

        // Marca (Giant/Trek/etc.).
        public string Marca { get; set; } = string.Empty;

        // Modelo / referencia.
        public string Modelo { get; set; } = string.Empty;

        // Color. Puede ser nulo.
        public string? Color { get; set; }

        // Número de serie del cuadro. Puede ser nulo.
        public string? NumeroSerie { get; set; }

        // Descripción general (estado, accesorios, detalle de servicio).
        public string? Descripcion { get; set; }

        // Fecha en que llegó al taller.
        public DateTime FechaIngreso { get; set; }

        // FK al dueño. Lo mantengo porque sirve para editar/filtrar desde el front.
        public int ClienteId { get; set; }

        // Nombre completo (Nombre + " " + Apellido) del cliente. Lo "aplanamos" en
        // este DTO para ahorrar una consulta adicional desde la UI.
        public string? ClienteNombre { get; set; }
    }
}
