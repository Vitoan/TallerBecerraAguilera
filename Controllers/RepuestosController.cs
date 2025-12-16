using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Helpers;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using Microsoft.AspNetCore.Authorization;

namespace TallerBecerraAguilera.Controllers
{
    public class RepuestosController : Controller
    {
        private readonly RepuestoRepositorio _repo;
        private readonly ProveedorRepositorio _proveedorRepo;

        public RepuestosController(RepuestoRepositorio repo, ProveedorRepositorio proveedorRepo)
        {
            _repo = repo;
            _proveedorRepo = proveedorRepo;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, string stock = "")
        {
            int pageSize = 10;

            var query = _repo.Query();

            if (stock == "con")
            {
                query = query.Where(r => r.cantidadStock > 0);
            }
            else if (stock == "sin")
            {
                query = query.Where(r => r.cantidadStock <= 0);
            }

            query = query.OrderBy(r => r.codigo);

            var paginated = await PaginatedList<Repuestos>
               .CreateAsync(query, pageNumber, pageSize);

            ViewBag.StockSeleccionado = stock;

            return View(paginated);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Create(Repuestos repuesto)
        {
            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }

            await _repo.AddAsync(repuesto);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var repuesto = await _repo.GetByIdAsync(id);
            if (repuesto == null) return NotFound();
            return View(repuesto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Edit(Repuestos repuesto)
        {
            if (!ModelState.IsValid)
            {
                return View(repuesto);
            }

            await _repo.UpdateAsync(repuesto);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var repuesto = await _repo.GetByIdAsync(id);
            return View(repuesto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> BuscarProveedores(string term)
        {
            var proveedores = await _proveedorRepo.Query()
                .Where(p => p.Nombre.Contains(term))
                .Select(p => new {
                    id = p.Id,
                    text = p.Nombre
                }).ToListAsync();

            return Json(new { results = proveedores });
        }
    }
}
