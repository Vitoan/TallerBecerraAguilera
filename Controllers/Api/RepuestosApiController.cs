using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers.Api
{
    // Ruta base:
    // https://localhost:XXXX/api/RepuestosApi
    [Route("api/[controller]")]
    [ApiController]
    // 🔒 Todos los endpoints requieren JWT, excepto los que tengan [AllowAnonymous]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RepuestosApiController : ControllerBase
    {
        private readonly RepuestoRepositorio _repo;

        public RepuestosApiController(RepuestoRepositorio repo)
        {
            _repo = repo;
        }

        // -------------------------------------------------------------
        // URL:
        // GET https://localhost:XXXX/api/RepuestosApi/stock
        // Header requerido:
        // Authorization: Bearer <TOKEN>
        // -------------------------------------------------------------
        [HttpGet("stock")]
        public async Task<IActionResult> GetStock()
        {
            var repuestos = await _repo.GetAllAsync();

            var resultado = repuestos.Select(r => new {
                Id = r.id,
                Codigo = r.codigo,
                Descripcion = r.descripcion,
                Stock = r.cantidadStock,
                Precio = r.precioUnitario,
                Proveedor = r.Proveedor?.Nombre ?? "Sin proveedor"
            });

            return Ok(resultado);
        }

        // -------------------------------------------------------------
        // URL:
        // GET https://localhost:XXXX/api/RepuestosApi/test
        // (NO requiere token)
        // -------------------------------------------------------------
        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok("API Repuestos funcionando");
        }

        // -------------------------------------------------------------
        // URL:
        // GET https://localhost:XXXX/api/RepuestosApi/secure
        // Header requerido:
        // Authorization: Bearer <TOKEN>
        // -------------------------------------------------------------
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            return Ok(new
            {
                Mensaje = "Acceso autorizado con JWT",
                Usuario = User.Claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value,
                Rol = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value
            });
        }

        // -------------------------------------------------------------
        // URL:
        // GET https://localhost:XXXX/api/RepuestosApi/1
        // (reemplazar 1 por el ID del repuesto)
        // Header requerido:
        // Authorization: Bearer <TOKEN>
        // -------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null)
                return NotFound();

            return Ok(new
            {
                r.id,
                r.codigo,
                r.descripcion,
                Stock = r.cantidadStock,
                Precio = r.precioUnitario
            });
        }

        // -------------------------------------------------------------
        // URL:
        // GET https://localhost:XXXX/api/RepuestosApi/stock-bajo
        // GET https://localhost:XXXX/api/RepuestosApi/stock-bajo?limite=3
        // Header requerido:
        // Authorization: Bearer <TOKEN>
        // -------------------------------------------------------------
        [HttpGet("stock-bajo")]
        public async Task<IActionResult> StockBajo([FromQuery] int limite = 5)
        {
            var repuestos = await _repo.GetAllAsync();

            var resultado = repuestos
                .Where(r => r.cantidadStock <= limite)
                .Select(r => new
                {
                    r.codigo,
                    r.descripcion,
                    Stock = r.cantidadStock
                });

            return Ok(resultado);
        }
    }
}
