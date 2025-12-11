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

        // GET: Mostrar formulario para agregar un repuesto
        public async Task<IActionResult> Create(int pedidoId)
        {
            // Creamos el modelo inicializado con el ID del padre para no perderlo
            var modelo = new PedidoRepuestos { pedido_id = pedidoId };

            // CORRECCIÓN IMPORTANTE: Tu modelo 'Repuestos' usa "Descripcion", no "Nombre".
            ViewBag.Repuestos = (await _repuestoRepo.GetAllAsync())
                .Select(r => new SelectListItem
                {
                    Value = r.id.ToString(),
                    Text = r.descripcion
                }).ToList();
            
            return View(modelo);
        }

        // POST: Guardar el repuesto en la base de datos
        [HttpPost]
        public async Task<IActionResult> Create(PedidoRepuestos item)
        {
            if (!ModelState.IsValid)
            {
                // Si falla, recargamos la lista. Nota: "Descripcion" aquí también.
                ViewBag.Repuestos = (await _repuestoRepo.GetAllAsync())
                    .Select(r => new SelectListItem
                    {
                        Value = r.id.ToString(),
                        Text = r.descripcion
                    }).ToList();
                return View(item);
            }

            try 
            {
                await _repo.AddAsync(item);
            }
            catch (Exception)
            {
                // Si intentan agregar el mismo repuesto dos veces, capturamos el error
                ModelState.AddModelError("", "Este repuesto ya está en la lista. Edítalo en lugar de agregarlo de nuevo.");
                ViewBag.Repuestos = (await _repuestoRepo.GetAllAsync())
                    .Select(r => new SelectListItem
                    {
                        Value = r.id.ToString(),
                        Text = r.descripcion
                    }).ToList();
                return View(item);
            }

            // CAMBIO CLAVE: Al guardar, volvemos a los DETALLES del Pedido Padre
            // para ver la lista completa actualizada.
            return RedirectToAction("Details", "PedidosRepuestos", new { id = item.pedido_id });
        }
    }
}