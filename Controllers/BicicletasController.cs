using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Control_Taller_Bicicletas.Data;
using Sistema_Control_Taller_Bicicletas.DTOs;
using Sistema_Control_Taller_Bicicletas.Models;

namespace Sistema_Control_Taller_Bicicletas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BicicletasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BicicletasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BicicletaDto>>> GetBicicletas()
        {
            var bicicletas = await _context.Bicicletas
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
                    ClienteNombre = b.Cliente != null ? b.Cliente.Nombre + " " + b.Cliente.Apellido : null
                })
                .ToListAsync();

            return Ok(bicicletas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BicicletaDto>> GetBicicleta(int id)
        {
            var bicicleta = await _context.Bicicletas
                .Include(b => b.Cliente)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bicicleta == null)
            {
                return NotFound();
            }

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

        [HttpPost]
        public async Task<ActionResult<BicicletaDto>> PostBicicleta(CrearBicicletaDto dto)
        {
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            if (!clienteExiste)
            {
                return BadRequest("El cliente especificado no existe.");
            }

            var bicicleta = new Bicicleta
            {
                Marca = dto.Marca,
                Modelo = dto.Modelo,
                Color = dto.Color,
                NumeroSerie = dto.NumeroSerie,
                Descripcion = dto.Descripcion,
                ClienteId = dto.ClienteId,
                FechaIngreso = DateTime.Now
            };

            _context.Bicicletas.Add(bicicleta);
            await _context.SaveChangesAsync();

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

            return CreatedAtAction(nameof(GetBicicleta), new { id = bicicleta.Id }, bicicletaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBicicleta(int id, CrearBicicletaDto dto)
        {
            var bicicleta = await _context.Bicicletas.FindAsync(id);
            if (bicicleta == null)
            {
                return NotFound();
            }

            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
            if (!clienteExiste)
            {
                return BadRequest("El cliente especificado no existe.");
            }

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

        [HttpGet("bycliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<BicicletaDto>>> GetBicicletasByCliente(int clienteId)
        {
            var bicicletas = await _context.Bicicletas
                .Include(b => b.Cliente)
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

        private bool BicicletaExists(int id)
        {
            return _context.Bicicletas.Any(e => e.Id == id);
        }
    }
}
