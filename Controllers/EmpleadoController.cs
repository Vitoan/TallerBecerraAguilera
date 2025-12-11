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

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            var empleados = await _repo.GetAllAsync();
            return View(empleados);
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var empleado = await _repo.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
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

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _repo.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        // POST: Empleados/Edit/5
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

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _repo.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
