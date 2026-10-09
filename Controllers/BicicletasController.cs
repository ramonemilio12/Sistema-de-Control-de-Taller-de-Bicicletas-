using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Control_Taller_Bicicletas.Data;
using Sistema_Control_Taller_Bicicletas.DTOs;
using Sistema_Control_Taller_Bicicletas.Models;

namespace Sistema_Control_Taller_Bicicletas.Controllers
{
    // Ruta base: api/Bicicletas. Mismo estilo de siempre: [ApiController] para que
    // haga la validación de DTOs y [Route] con el nombre convencional.
    [Route("api/[controller]")]
    [ApiController]
    public class BicicletasController : ControllerBase
    {
        // DbContext inyectado por DI.
        private readonly ApplicationDbContext _context;

        public BicicletasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Bicicletas
        // Lista TODAS las bicicletas del taller. Incluyo la propiedad de navegación
        // Cliente solo para poder concatenar nombre+apellido en el DTO (no envío el
        // objeto entero Cliente).
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BicicletaDto>>> GetBicicletas()
        {
            var bicicletas = await _context.Bicicletas
                // Include(b => b.Cliente) hace un JOIN en SQL para traer al dueño.
                // Si no lo hiciera, "b.Cliente" quedaría null y ClienteNombre llegaría
                // vacío en el response.
                .Include(b => b.Cliente)
                .Select(b => new BicicletaDto
                {
                    Id = b.Id,
                    Marca = b.Marca,
                    Modelo = b.Modelo,
                    Color = b.Color,
                    NumeroSerie = b.NumeroSerie,
                    Descripcion = b.Descripcion,
                    FechaIngreso = b.FechaIngreso,
                    ClienteId = b.ClienteId,
                    // Uso concatenación simple; si quieres podrías hacer un helper
                    // por ejemplo $"{Cliente.Nombre} {Cliente.Apellido}".
                    ClienteNombre = b.Cliente != null ? b.Cliente.Nombre + " " + b.Cliente.Apellido : null
                })
                .ToListAsync();

            return Ok(bicicletas);
        }

        // GET: api/Bicicletas/3
        // Detalle de UNA sola bicicleta por id. Incluye también al dueño para
        // mostrar su nombre completo.
        [HttpGet("{id}")]
        public async Task<ActionResult<BicicletaDto>> GetBicicleta(int id)
        {
            var bicicleta = await _context.Bicicletas
                .Include(b => b.Cliente)
                // Uso FirstOrDefaultAsync porque hay Include (no puedo usar FindAsync).
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bicicleta == null)
            {
                return NotFound();
            }

            // Mapeo manual a BicicletaDto.
            var bicicletaDto = new BicicletaDto
            {
                Id = bicicleta.Id,
                Marca = bicicleta.Marca,
                Modelo = bicicleta.Modelo,
                Color = bicicleta.Color,
                NumeroSerie = bicicleta.NumeroSerie,
                Descripcion = bicicleta.Descripcion,
                FechaIngreso = bicicleta.FechaIngreso,
                ClienteId = bicicleta.ClienteId,
                ClienteNombre = bicicleta.Cliente != null ? bicicleta.Cliente.Nombre + " " + bicicleta.Cliente.Apellido : null
            };

            return Ok(bicicletaDto);
        }

        // POST: api/Bicicletas
        // Crea una bicicleta nueva y la asigna a un cliente existente. Antes de
        // guardar, valido que ClienteId exista de verdad para evitar violaciones de
        // FK en SQL Server (que si pasan, el error es mucho más feo).
        [HttpPost]
        public async Task<ActionResult<BicicletaDto>> PostBicicleta(CrearBicicletaDto dto)
        {
            // Precondición: el dueño debe existir.
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            if (!clienteExiste)
            {
                return BadRequest("El cliente especificado no existe.");
            }

            // Mapeo DTO -> entidad.
            var bicicleta = new Bicicleta
            {
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Color = dto.Color,
                NumeroSerie = dto.NumeroSerie,
                Descripcion = dto.Descripcion,
                ClienteId = dto.ClienteId,
                // FechaIngreso la establece el servidor, no el cliente.
                FechaIngreso = DateTime.Now
            };

            _context.Bicicletas.Add(bicicleta);
            await _context.SaveChangesAsync();

            // Devuelvo un BicicletaDto. Aquí NO hice Include() por lo que
            // ClienteNombre llega nulo; lo normal es que el front después vuelva a
            // hacer GET /api/bicicletas/{id} para traerlo completo.
            var bicicletaDto = new BicicletaDto
            {
                Id = bicicleta.Id,
                Marca = bicicleta.Marca,
                Modelo = bicicleta.Modelo,
                Color = bicicleta.Color,
                NumeroSerie = bicicleta.NumeroSerie,
                Descripcion = bicicleta.Descripcion,
                FechaIngreso = bicicleta.FechaIngreso,
                ClienteId = bicicleta.ClienteId
            };

            // 201 Created con la URL de detalle.
            return CreatedAtAction(nameof(GetBicicleta), new { id = bicicleta.Id }, bicicletaDto);
        }

        // PUT: api/Bicicletas/3
        // Actualiza una bicicleta existente. Misma idea que el PUT de Clientes, pero
        // aquí además validamos que el nuevo ClienteId exista.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBicicleta(int id, CrearBicicletaDto dto)
        {
            var bicicleta = await _context.Bicicletas.FindAsync(id);
            if (bicicleta == null)
            {
                return NotFound();
            }

            // Si el usuario le cambió el dueño, que el nuevo dueño sea válido.
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            if (!clienteExiste)
            {
                return BadRequest("El cliente especificado no existe.");
            }

            // Sobreescritura de propiedades. No actualizo FechaIngreso porque es el
            // histórico de cuándo entró; si queremos un "fecha última actualización"
            // se podría agregar una columna aparte.
            bicicleta.Marca = dto.Marca;
            bicicleta.Modelo = dto.Modelo;
            bicicleta.Color = dto.Color;
            bicicleta.NumeroSerie = dto.NumeroSerie;
            bicicleta.Descripcion = dto.Descripcion;
            bicicleta.ClienteId = dto.ClienteId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BicicletaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Bicicletas/3
        // Borra una sola bicicleta. Esto NO afecta al cliente ni a otras bicicletas
        // del mismo (a diferencia de borrar cliente, que sí borra en cascada).
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBicicleta(int id)
        {
            var bicicleta = await _context.Bicicletas.FindAsync(id);
            if (bicicleta == null)
            {
                return NotFound();
            }

            _context.Bicicletas.Remove(bicicleta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Bicicletas/bycliente/2
        // Endpoint adicional muy útil: "dame TODAS las bicicletas de este cliente".
        // Se usa mucho cuando el usuario está viendo la ficha de un cliente y quiere
        // desplegar sus bicicletas.
        [HttpGet("bycliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<BicicletaDto>>> GetBicicletasByCliente(int clienteId)
        {
            var bicicletas = await _context.Bicicletas
                .Include(b => b.Cliente)
                // Filtra por el id de cliente que nos pasaron por URL.
                .Where(b => b.ClienteId == clienteId)
                .Select(b => new BicicletaDto
                {
                    Id = b.Id,
                    Marca = b.Marca,
                    Modelo = b.Modelo,
                    Color = b.Color,
                    NumeroSerie = b.NumeroSerie,
                    Descripcion = b.Descripcion,
                    FechaIngreso = b.FechaIngreso,
                    ClienteId = b.ClienteId,
                    ClienteNombre = b.Cliente != null ? b.Cliente.Nombre + " " + b.Cliente.Apellido : null
                })
                .ToListAsync();

            return Ok(bicicletas);
        }

        // Helper: ¿existe esta bicicleta? Usado en el PUT.
        private bool BicicletaExists(int id)
        {
            return _context.Bicicletas.Any(e => e.Id == id);
        }
    }
}
