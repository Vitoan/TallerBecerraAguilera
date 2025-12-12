using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers
{
    public class PedidoRepuestosController : Controller
    {
        private readonly PedidoRepuestosRepositorio _repo;
        private readonly PedidosRepuestosRepositorio _pedidoRepo; // para validar estado
        private readonly RepuestoRepositorio _repuestoRepo;

        public PedidoRepuestosController(
            PedidoRepuestosRepositorio repo,
            PedidosRepuestosRepositorio pedidoRepo,
            RepuestoRepositorio repuestoRepo)
        {
            _repo = repo;
            _pedidoRepo = pedidoRepo;
            _repuestoRepo = repuestoRepo;
        }

        // GET: Formulario para agregar repuesto al pedido
        public async Task<IActionResult> Create(int pedidoId)
        {
            var modelo = new PedidoRepuestos { pedido_id = pedidoId };

            await CargarRepuestosAsync();

            return View(modelo);
        }

        // POST: Guardar nuevo ítem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoRepuestos item)
        {
            if (!ModelState.IsValid)
            {
                await CargarRepuestosAsync();
                return View(item);
            }

            try
            {
                await _repo.AddAsync(item);
                TempData["Success"] = "Repuesto agregado al pedido correctamente.";
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Este repuesto ya está en el pedido. Edítalo o elimínalo primero.");
                await CargarRepuestosAsync();
                return View(item);
            }

            return RedirectToAction("Details", "PedidosRepuestos", new { id = item.pedido_id });
        }

        // POST: Eliminar un repuesto del pedido
        [HttpPost]
        public async Task<IActionResult> Delete(int pedidoId, int repuestoId)
        {
            var pedido = await _pedidoRepo.GetByIdAsync(pedidoId);
            if (pedido == null)
                return NotFound();

            if (pedido.Estado != EstadoPedido.Pendiente)
            {
                TempData["Error"] = "No puedes eliminar repuestos de un pedido que ya no está pendiente.";
                return RedirectToAction("Details", "PedidosRepuestos", new { id = pedidoId });
            }

            await _repo.DeleteAsync(pedidoId, repuestoId);

            TempData["Success"] = "Repuesto eliminado del pedido.";

            // Soporte para AJAX
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }

            return RedirectToAction("Details", "PedidosRepuestos", new { id = pedidoId });
        }

        // Método auxiliar para cargar el dropdown
        private async Task CargarRepuestosAsync()
        {
            var repuestos = await _repuestoRepo.GetAllAsync();

            ViewBag.Repuestos = repuestos
                .Select(r => new SelectListItem
                {
                    Value = r.id.ToString(),
                    Text = r.descripcion ?? "(Sin descripción)"
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public async Task<IActionResult> GetRepuestos(string term)
        {
            var repuestos = await _repo.GetAllRepuestosAsync();

            var filtered = repuestos
                .Where(r => string.IsNullOrEmpty(term) || r.descripcion.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(r => new { id = r.id, text = r.descripcion });

            return Json(new { results = filtered });
        }
    }
}