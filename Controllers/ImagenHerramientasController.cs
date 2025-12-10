using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers
{
    public class ImagenHerramientasController : Controller
    {
        private readonly ImagenHerramientaRepositorio _repo;

        public ImagenHerramientasController(ImagenHerramientaRepositorio repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(int herramientaId)
        {
            var imagenes = await _repo.ObtenerPorHerramientaAsync(herramientaId);
            ViewBag.HerramientaId = herramientaId;
            return View(imagenes);
        }

        // ★★ NUEVO: Pantalla de subida
        public IActionResult Create(int herramientaId)
        {
            var modelo = new ImagenHerramienta
            {
                HerramientaId = herramientaId
            };
            return View(modelo);
        }

        // ★★ NUEVO: POST del formulario Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImagenHerramienta modelo)
        {
            if (modelo.Archivo == null)
            {
                ModelState.AddModelError("Archivo", "Debe seleccionar una imagen.");
                return View(modelo);
            }

            await _repo.GuardarAsync(modelo);

            return RedirectToAction("Index", new { herramientaId = modelo.HerramientaId });
        }

        [HttpPost]
        public async Task<IActionResult> Subir(int herramientaId, IFormFile archivo)
        {
            if (archivo == null)
            {
                TempData["Error"] = "Debe seleccionar una imagen.";
                return RedirectToAction("Index", new { herramientaId });
            }

            await _repo.GuardarAsync(new ImagenHerramienta
            {
                HerramientaId = herramientaId,
                Archivo = archivo
            });

            return RedirectToAction("Index", new { herramientaId });
        }

        public async Task<IActionResult> Eliminar(int id, int herramientaId)
        {
            await _repo.EliminarAsync(id);
            return RedirectToAction("Index", new { herramientaId });
        }
    }
}
