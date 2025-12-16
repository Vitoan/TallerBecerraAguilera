using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Helpers;
using Microsoft.AspNetCore.Authorization;


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

        public async Task<IActionResult> Index(EstadoHerramienta? estado, int pageNumber = 1)
        {
            int pageSize = 10;

            var query = _repo.Query().AsQueryable();

            if (estado.HasValue)
            {
                query = query.Where(h => h.Estado == estado.Value);
            }

            query = query.Include(h => h.Imagenes);

            query = query.OrderBy(h => h.Nombre);

            var paginated = await PaginatedList<Herramientas>.CreateAsync(query, pageNumber, pageSize);

            ViewBag.EstadoSeleccionado = estado;
            return View(paginated);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Estados = new SelectList(Enum.GetValues(typeof(EstadoHerramienta)));
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var herramienta = await _repo.GetByIdAsync(id);
            if (herramienta == null)
                return NotFound();

            ViewBag.Estados = new SelectList(Enum.GetValues(typeof(EstadoHerramienta)), herramienta.Estado);
            return View(herramienta);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
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
    // Llamamos al repositorio que devuelve un tuple (bool, string)
            var resultado = await _repo.DeleteAsync(id);

            // Eliminamos imágenes si la herramienta se borró correctamente
            if (resultado.ok)
            {
                var imagenes = await _imgRepo.ObtenerPorHerramientaAsync(id);
                foreach (var img in imagenes)
                {
                    await _imgRepo.EliminarAsync(img.Id);
                }

                TempData["success"] = resultado.mensaje;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = resultado.mensaje;
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}
