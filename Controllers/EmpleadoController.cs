using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TallerBecerraAguilera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoRepositorio _repo;

        public EmpleadosController(EmpleadoRepositorio repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 10;
            var empleados = await _repo.GetAllPaginatedAsync(pageNumber ?? 1, pageSize);
            return View(empleados);
        }

        public async Task<IActionResult> Details(int id)
        {
            var empleado = await _repo.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleados empleado)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddAsync(empleado);
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _repo.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleados empleado)
        {
            if (id != empleado.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await _repo.UpdateAsync(empleado);
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _repo.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (ok, mensaje) = await _repo.DeleteAsync(id);
            TempData["Mensaje"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}
