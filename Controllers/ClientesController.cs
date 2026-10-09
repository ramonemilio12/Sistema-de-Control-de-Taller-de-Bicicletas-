using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Control_Taller_Bicicletas.Data;
using Sistema_Control_Taller_Bicicletas.DTOs;
using Sistema_Control_Taller_Bicicletas.Models;

namespace Sistema_Control_Taller_Bicicletas.Controllers
{
    // Ruta base del controlador: "api/Clientes". El [ApiController] activa
    // validaciones automáticas (DTO con [Required] que lleguen vacíos devuelven
    // 400 sin que yo tenga que chequear ModelState.IsValid a mano).
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        // ApplicationDbContext se inyecta por constructor. La dependencia se registró
        // en Program.cs con AddDbContext. No hago "new ApplicationDbContext()"
        // porque me rompería la vida útil del contexto y no usaría el pool de EF.
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        // Devuelve TODOS los clientes. Lo hago con .Select(... ClienteDto ...) para
        // NO devolver la entidad directamente. Muy importante: aquí no uso
        // Include(c => c.Bicicletas) porque sería muy pesado si el cliente tiene 20
        // bicicletas; para eso hay endpoints específicos.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono,
                    Email = c.Email,
                    Direccion = c.Direccion,
                    FechaRegistro = c.FechaRegistro
                })
                .ToListAsync();

            // HTTP 200 con el JSON del listado.
            return Ok(clientes);
        }

        // GET: api/Clientes/5
        // Obtiene UN solo cliente por su PK. Si no existe retorna 404 (NotFound).
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            // FindAsync busca por la PK y usa el cache local del DbContext si lo
            // encuentra ahí. Rápido y óptimo para consultas "trivial by id".
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                // 404 - cliente inexistente.
                return NotFound();
            }

            // Convierto manualmente la entidad a DTO (más tarde podríamos reemplazar
            // este código repetido por Mapster o AutoMapper, pero por ahora lo
            // mantenemos "manual" para que el proyecto no tenga dependencias extra).
            var clienteDto = new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Direccion = cliente.Direccion,
                FechaRegistro = cliente.FechaRegistro
            };

            return Ok(clienteDto);
        }

        // POST: api/Clientes
        // Crea un cliente NUEVO. Recibe un CrearClienteDto (sin Id, sin FechaRegistro),
        // lo transforma en entidad, lo guarda y devuelve el ClienteDto recién creado
        // con el Id que SQL le asignó.
        [HttpPost]
        public async Task<ActionResult<ClienteDto>> PostCliente(CrearClienteDto dto)
        {
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion,
                // FechaRegistro la seteamos AQUÍ en el servidor, NO la aceptamos del
                // usuario; así evitamos que alguien registre clientes con fechas falsas.
                FechaRegistro = DateTime.Now
            };

            // Agregamos la entidad al Change Tracker de EF (aún no va a SQL).
            _context.Clientes.Add(cliente);
            // AQUÍ se ejecuta el INSERT real y vuelve a poblar cliente.Id con el
            // identity generado por la base de datos.
            await _context.SaveChangesAsync();

            // Volvemos a mapear hacia DTO para incluir el nuevo Id en la respuesta.
            var clienteDto = new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                Direccion = cliente.Direccion,
                FechaRegistro = cliente.FechaRegistro
            };

            // Retornamos 201 Created con la URL para volver a buscar el cliente
            // (GET api/clientes/123) y en el body el DTO recién creado.
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, clienteDto);
        }

        // PUT: api/Clientes/5
        // Actualiza los datos de un cliente EXISTENTE. Se usa el mismo DTO de crear
        // porque los campos son los mismos; solo cambia que ahora SÍ necesitamos el id.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, CrearClienteDto dto)
        {
            // 1) Lo buscamos en la base por id.
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            // 2) Sobreescribimos propiedades por los valores que llegó en el body.
            cliente.Nombre = dto.Nombre;
            cliente.Apellido = dto.Apellido;
            cliente.Telefono = dto.Telefono;
            cliente.Email = dto.Email;
            cliente.Direccion = dto.Direccion;

            // 3) Guardamos. Si hay concurrencia (otro usuario borró el mismo registro
            // mientras nosotros lo editábamos) EF tira DbUpdateConcurrencyException.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Volvemos a chequear existencia para decidir si es un 404 o un
                // error real.
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    // Cualquier otro problema de concurrencia lo dejo subir hasta un
                    // middleware global de manejo de excepciones (cuando lo tengamos).
                    throw;
                }
            }

            // Respuesta 204 No Content: "todo bien, pero no devuelvo nada".
            return NoContent();
        }

        // DELETE: api/Clientes/5
        // Elimina un cliente. Importante: por la configuración OnDelete(DeleteBehavior.Cascade)
        // que tenemos en ApplicationDbContext, si borramos el cliente también se
        // BORRAN sus bicicletas asociadas. No hay borrado suave por ahora.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            // Marca la entidad como Deleted en el Change Tracker.
            _context.Clientes.Remove(cliente);
            // Execute DELETE físico en SQL (más el delete en cascada).
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método helper privado. Se usa en el PUT para saber si el cliente sigue
        // existiendo después de un error de concurrencia.
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
