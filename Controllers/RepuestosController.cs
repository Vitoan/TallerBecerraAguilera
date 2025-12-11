using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Helpers;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

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

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;

            var query = _repo.Query()
                             .OrderBy(r => r.codigo);
            
            var paginated = PaginatedList<Repuestos>.CreateAsync(query, pageNumber, pageSize);

            return View(paginated);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Repuestos repuesto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre");
                return View(repuesto);
            }

            await _repo.AddAsync(repuesto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var repuesto = await _repo.GetByIdAsync(id);
            ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre", repuesto?.proveedorId);
            return View(repuesto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Repuestos repuesto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProveedorId = new SelectList(await _proveedorRepo.GetAllAsync(), "Id", "Nombre", repuesto.proveedorId);
                return View(repuesto);
            }

            await _repo.UpdateAsync(repuesto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var repuesto = await _repo.GetByIdAsync(id);
            return View(repuesto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
