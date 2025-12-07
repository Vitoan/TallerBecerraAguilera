using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ProveedorRepositorio _repo;

        public ProveedoresController(ProveedorRepositorio repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Proveedores proveedor)
        {
            if (!ModelState.IsValid) return View(proveedor);
            await _repo.AddAsync(proveedor);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var proveedor = await _repo.GetByIdAsync(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Proveedores proveedor)
        {
            if (!ModelState.IsValid) return View(proveedor);
            await _repo.UpdateAsync(proveedor);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _repo.GetByIdAsync(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
