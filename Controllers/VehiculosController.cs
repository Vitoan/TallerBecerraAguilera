using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;

namespace TallerBecerraAguilera.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly VehiculoRepositorio _vehiculoRepositorio;

        public VehiculosController(VehiculoRepositorio vehiculoRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
        }

        // ===========================
        // GET: Vehiculos
        // ===========================
        public async Task<IActionResult> Index()
        {
            var vehiculos = await _vehiculoRepositorio.GetAllAsync();
            return View(vehiculos);
        }

        // ===========================
        // GET: Vehiculos/Details/5
        // ===========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vehiculo = await _vehiculoRepositorio.GetByIdAsync(id.Value);
            if (vehiculo == null) return NotFound();

            return View(vehiculo);
        }

        // ===========================
        // GET: Vehiculos/Create
        // ===========================
        public async Task<IActionResult> Create()
        {
            await PopulateClientesDropDownList();
            return View();
        }

        // ===========================
        // POST: Vehiculos/Create
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculos vehiculo)
        {
            if (await _vehiculoRepositorio.GetByPatenteAsync(vehiculo.Patente) != null)
            {
                ModelState.AddModelError("Patente", "La patente ya está registrada en el sistema.");
            }

            if (ModelState.IsValid)
            {
                await _vehiculoRepositorio.AddAsync(vehiculo);
                return RedirectToAction(nameof(Index));
            }

            await PopulateClientesDropDownList(vehiculo.ClienteId);
            return View(vehiculo);
        }

        // ===========================
        // GET: Vehiculos/Edit/5
        // ===========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vehiculo = await _vehiculoRepositorio.GetByIdAsync(id.Value);
            if (vehiculo == null) return NotFound();

            await PopulateClientesDropDownList(vehiculo.ClienteId);
            return View(vehiculo);
        }

        // ===========================
        // POST: Vehiculos/Edit/5
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehiculos vehiculo)
        {
            if (id != vehiculo.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _vehiculoRepositorio.UpdateAsync(vehiculo);
                return RedirectToAction(nameof(Index));
            }

            await PopulateClientesDropDownList(vehiculo.ClienteId);
            return View(vehiculo);
        }

        // ===========================
        // GET: Vehiculos/Delete/5
        // ===========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vehiculo = await _vehiculoRepositorio.GetByIdAsync(id.Value);
            if (vehiculo == null) return NotFound();

            return View(vehiculo);
        }

        // ===========================
        // POST: Vehiculos/Delete/5
        // ===========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehiculoRepositorio.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // DROPDOWN DE CLIENTES
        // ===========================
        private async Task PopulateClientesDropDownList(object? selectedCliente = null)
        {
            var clientes = await _vehiculoRepositorio.GetAllClientesAsync();
            ViewBag.ClienteId = new SelectList(clientes, "Id", "NombreCompleto", selectedCliente);
        }
    }
}
