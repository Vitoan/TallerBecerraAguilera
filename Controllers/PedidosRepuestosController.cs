using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers
{
    public class PedidosRepuestosController : Controller
    {
        private readonly PedidosRepuestosRepositorio _repo;
        private readonly ProveedorRepositorio _proveedorRepo;

        public PedidosRepuestosController(PedidosRepuestosRepositorio repo, ProveedorRepositorio proveedorRepo)
        {
            _repo = repo;
            _proveedorRepo = proveedorRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PedidosRepuestos pedido)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre");
                return View(pedido);
            }

            var id = await _repo.AddAsync(pedido);
            return RedirectToAction("Create", "PedidoRepuestos", new { pedidoId = id });
        }

        public async Task<IActionResult> CambiarEstado(int id, EstadoPedido estado)
        {
            await _repo.CambiarEstadoAsync(id, estado);
            return RedirectToAction(nameof(Index));
        }
    }
}
