using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers
{
    public class HerramientasController : Controller
    {
        private readonly HerramientaRepositorio _repo;

        public HerramientasController(HerramientaRepositorio repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Estados = new SelectList(Enum.GetValues(typeof(EstadoHerramienta)));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Herramientas herramienta)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddAsync(herramienta);
                TempData["success"] = "Herramienta creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Estados = new SelectList(Enum.GetValues(typeof(EstadoHerramienta)));
            return View(herramienta);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var herramienta = await _repo.GetByIdAsync(id);
            if (herramienta == null)
                return NotFound();

            ViewBag.Estados = new SelectList(Enum.GetValues(typeof(EstadoHerramienta)), herramienta.Estado);
            return View(herramienta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Herramientas herramienta)
        {
            if (ModelState.IsValid)
            {
                herramienta.Updated_at = DateTime.Now;
                await _repo.UpdateAsync(herramienta);
                TempData["success"] = "Herramienta actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Estados = new SelectList(Enum.GetValues(typeof(EstadoHerramienta)), herramienta.Estado);
            return View(herramienta);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var herramienta = await _repo.GetByIdAsync(id);
            if (herramienta == null)
                return NotFound();

            return View(herramienta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            TempData["success"] = "Herramienta eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
