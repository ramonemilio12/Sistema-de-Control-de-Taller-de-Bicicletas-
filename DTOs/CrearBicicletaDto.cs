using System.ComponentModel.DataAnnotations;

namespace Sistema_Control_Taller_Bicicletas.DTOs
{
    // DTO de ESCRITURA para CREAR una bicicleta nueva. Al igual que CrearClienteDto,
    // NO trae Id ni FechaIngreso ni la navegación Cliente.
    // Solo le pido al front lo mínimo necesario para registrar la bici.
    public class CrearBicicletaDto
    {
        // Marca obligatoria.
        [Required(ErrorMessage = "La marca es obligatoria.")]
        [MaxLength(50, ErrorMessage = "La marca no debe exceder los 50 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        // Modelo obligatorio.
        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El modelo no debe exceder los 50 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        // Color opcional.
        [MaxLength(20, ErrorMessage = "El color no debe exceder los 20 caracteres.")]
        public string? Color { get; set; }

        // Número de serie opcional (muchas bicicletas chinas no lo tienen impreso).
        [MaxLength(50, ErrorMessage = "El número de serie no debe exceder los 50 caracteres.")]
        public string? NumeroSerie { get; set; }

        // Notas libres.
        [MaxLength(500, ErrorMessage = "La descripción no debe exceder los 500 caracteres.")]
        public string? Descripcion { get; set; }

        // AQUÍ SÍ es obligatorio el ClienteId: no puedo guardar una bici sin dueño.
        [Required(ErrorMessage = "Debe indicar a qué cliente pertenece la bicicleta.")]
        public int ClienteId { get; set; }
    }
}
