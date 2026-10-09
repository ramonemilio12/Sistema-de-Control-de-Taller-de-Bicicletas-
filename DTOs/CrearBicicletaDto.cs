using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    // Cuerpo de la petición para crear una bicicleta.
    public class CrearBicicletaDto
    {
        [Required(ErrorMessage = "La marca es obligatoria.")]
        [MaxLength(50, ErrorMessage = "La marca no debe exceder los 50 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El modelo no debe exceder los 50 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "El color no debe exceder los 20 caracteres.")]
        public string? Color { get; set; }

        [MaxLength(50, ErrorMessage = "El número de serie no debe exceder los 50 caracteres.")]
        public string? NumeroSerie { get; set; }

        [MaxLength(500, ErrorMessage = "La descripción no debe exceder los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Debe indicar a qué cliente pertenece la bicicleta.")]
        public int ClienteId { get; set; }
    }
}
