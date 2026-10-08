using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Control_Taller_Bicicletas.Models
{
    public class Bicicleta
    {
        [Key]
        public int Id { get; set; }

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

        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }
    }
}
