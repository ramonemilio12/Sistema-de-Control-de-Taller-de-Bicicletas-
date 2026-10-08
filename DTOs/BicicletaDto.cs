namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    public class BicicletaDto
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? NumeroSerie { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int ClienteId { get; set; }
        public string? ClienteNombre { get; set; }
    }
}
