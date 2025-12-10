using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers
{
    public class HerramientasController : Controller
    {
        private readonly HerramientaRepositorio _repo;
        private readonly ImagenHerramientaRepositorio _imgRepo;

        public HerramientasController(HerramientaRepositorio repo, ImagenHerramientaRepositorio imgRepo)
        {
            _repo = repo;
            _imgRepo = imgRepo;
        }

        public async Task<IActionResult> Index()
        {
            // Incluye imágenes para mostrar miniaturas
            var herramientas = await _repo.Query()
                .Include(h => h.Imagenes)
                .ToListAsync();

            return View(herramientas);
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
                herramienta.Created_at = DateTime.Now;
                herramienta.Updated_at = DateTime.Now;

                await _repo.AddAsync(herramienta);
                TempData["success"] = "Herramienta creada correctamente.";

                // Redirige directo a subir imágenes
                return RedirectToAction("Index", "ImagenHerramientas", new { herramientaId = herramienta.Id });
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

            // También eliminamos imágenes de la herramienta
            var imagenes = await _imgRepo.ObtenerPorHerramientaAsync(id);
            foreach (var img in imagenes)
            {
                await _imgRepo.EliminarAsync(img.Id);
            }

            TempData["success"] = "Herramienta eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
