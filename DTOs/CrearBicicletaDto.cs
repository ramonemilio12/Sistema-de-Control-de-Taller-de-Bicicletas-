using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    public class CrearBicicletaDto
    {
        [Required]
        [MaxLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Color { get; set; }

        [MaxLength(50)]
        public string? NumeroSerie { get; set; }

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public int ClienteId { get; set; }
    }
}
