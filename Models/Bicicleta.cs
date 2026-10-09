using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Control_Taller_Bicicletas.Models
{
    public class Bicicleta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La marca de la bicicleta es obligatoria.")]
        [MaxLength(50, ErrorMessage = "La marca no debe superar los 50 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo de la bicicleta es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El modelo no debe superar los 50 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "El color no debe superar los 20 caracteres.")]
        public string? Color { get; set; }

        [MaxLength(50, ErrorMessage = "El número de serie no debe superar los 50 caracteres.")]
        public string? NumeroSerie { get; set; }

        [MaxLength(500, ErrorMessage = "La descripción no debe superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }
    }
}
