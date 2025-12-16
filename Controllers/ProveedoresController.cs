using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TallerBecerraAguilera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProveedoresController : Controller
    {
        private readonly ProveedorRepositorio _repo;

        public ProveedoresController(ProveedorRepositorio repo)
        {
            _repo = repo;
        }

        // GET: Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Listar todos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _repo.GetAllAsync();
            return Json(lista);
        }

        // POST: Agregar
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Proveedores proveedor)
        {
            if (proveedor == null || string.IsNullOrWhiteSpace(proveedor.Nombre))
                return BadRequest("Faltan datos del proveedor");

            var existente = await _repo.GetByNameAsync(proveedor.Nombre);
            if (existente != null)
                return BadRequest("Ya existe un proveedor con ese nombre");

            await _repo.AddAsync(proveedor);
            return Json(proveedor);
        }

        // POST: Editar
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Proveedores proveedor)
        {
            if (proveedor == null || proveedor.Id == 0 || string.IsNullOrWhiteSpace(proveedor.Nombre))
                return BadRequest("Faltan datos del proveedor");

            await _repo.UpdateAsync(proveedor); // Repositorio ya hace fetch y update seguro
            return Json(proveedor);
        }

        // POST: Eliminar
        [HttpPost]
public async Task<IActionResult> Delete([FromBody] Proveedores proveedor)
{
    try
    {
        await _repo.DeleteAsync(proveedor.Id);
        return Ok();
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(ex.Message);
    }
}

    }
}
