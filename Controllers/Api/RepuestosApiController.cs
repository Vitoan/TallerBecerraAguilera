using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers.Api
{
    // Ruta de acceso: https://tusitio.com/api/RepuestosApi
    [Route("api/[controller]")]
    [ApiController]
    // 🔒 SEGURIDAD: Esto obliga a que la petición tenga un Header "Authorization: Bearer <TOKEN>"
    // Si intentas entrar por el navegador sin token, te dará error 401 (No autorizado).
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    public class RepuestosApiController : ControllerBase
    {
        private readonly RepuestoRepositorio _repo;

        public RepuestosApiController(RepuestoRepositorio repo)
        {
            _repo = repo;
        }

        // GET: api/RepuestosApi/stock
        [HttpGet("stock")]
        public async Task<IActionResult> GetStock()
        {
            var repuestos = await _repo.GetAllAsync();

            // PROYECCIÓN (Buena práctica):
            // No devolvemos la entidad completa (con relaciones cíclicas o datos privados).
            // Creamos una lista anónima limpia solo con lo que la API necesita.
            var resultado = repuestos.Select(r => new {
                Id = r.id,
                Codigo = r.codigo,
                Descripcion = r.descripcion,
                Stock = r.cantidadStock,
                Precio = r.precioUnitario,
                Proveedor = r.Proveedor?.Nombre ?? "Sin proveedor"
            });

            return Ok(resultado); // Devuelve JSON con código 200 OK
        }
    }
}