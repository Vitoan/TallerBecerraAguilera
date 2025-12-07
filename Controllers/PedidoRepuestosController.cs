using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers
{
    public class PedidoRepuestosController : Controller
    {
        private readonly PedidoRepuestosRepositorio _repo;
        private readonly RepuestoRepositorio _repuestoRepo;

        public PedidoRepuestosController(PedidoRepuestosRepositorio repo, RepuestoRepositorio repuestoRepo)
        {
            _repo = repo;
            _repuestoRepo = repuestoRepo;
        }

        public async Task<IActionResult> Create(int pedidoId)
        {
            ViewBag.PedidoId = pedidoId;
            ViewBag.RepuestoId = new SelectList(await _repuestoRepo.GetAllAsync(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PedidoRepuestos item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RepuestoId = new SelectList(await _repuestoRepo.GetAllAsync(), "Id", "Nombre");
                return View(item);
            }

            await _repo.AddAsync(item);
            return RedirectToAction("Create", new { pedidoId = item.PedidoId });
        }

        public async Task<IActionResult> Recibir(int pedidoId, int repuestoId, int cantidad)
        {
            await _repo.RecibirRepuestoAsync(pedidoId, repuestoId, cantidad);
            return RedirectToAction("Index", "PedidosRepuestos");
        }
    }
}
