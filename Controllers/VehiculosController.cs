using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
// NECESARIO para DbUpdateConcurrencyException y otros errores de EF Core.
using Microsoft.EntityFrameworkCore; 

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
        // GET: Vehiculos (Listar)
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

        // GET: Vehiculos/Create
        public IActionResult Create() // Quitamos el async y el Task porque ya no esperamos nada
        {
            // ELIMINAMOS O COMENTAMOS ESTA LÍNEA:
            // await PopulateClientesDropDownList(); 
            // Ya no enviamos la lista llena, el front la pedirá por demanda.
            
            return View();
        }

        // ===========================
        // POST: Vehiculos/Create (Guardar Nuevo)
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculos vehiculo)
        {
            // Poner patente en mayúsculas para consistencia y búsqueda
            vehiculo.Patente = vehiculo.Patente.ToUpper();

            // 1. Validar unicidad de Patente
            if (await _vehiculoRepositorio.GetByPatenteAsync(vehiculo.Patente) != null)
            {
                ModelState.AddModelError("Patente", "La patente ya está registrada en el sistema.");
            }

            if (ModelState.IsValid)
            {
                await _vehiculoRepositorio.AddAsync(vehiculo);
                TempData["Mensaje"] = "Vehículo registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateClientesDropDownList(vehiculo.ClienteId);
            return View(vehiculo);
        }

        // ===========================
        // GET: Vehiculos/Edit/5 (Mostrar Formulario)
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
        // POST: Vehiculos/Edit/5 (Guardar Cambios)
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehiculos vehiculo)
        {
            if (id != vehiculo.Id) return NotFound();

            // Poner patente en mayúsculas para consistencia
            vehiculo.Patente = vehiculo.Patente.ToUpper();

            if (ModelState.IsValid)
            {
                // 2. Validación de unicidad de patente en la edición
                var existingVehiculo = await _vehiculoRepositorio.GetByPatenteAsync(vehiculo.Patente);

                // Si existe un vehículo con esa patente Y NO es el vehículo que estamos editando
                if (existingVehiculo != null && existingVehiculo.Id != vehiculo.Id)
                {
                    ModelState.AddModelError("Patente", "La patente ya está registrada para otro vehículo.");
                    await PopulateClientesDropDownList(vehiculo.ClienteId);
                    return View(vehiculo);
                }

                try
                {
                    await _vehiculoRepositorio.UpdateAsync(vehiculo);
                    TempData["Mensaje"] = "Vehículo actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException) // <--- ERROR CORREGIDO con el 'using'
                {
                    if (await _vehiculoRepositorio.ExistsAsync(vehiculo.Id) == false)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateClientesDropDownList(vehiculo.ClienteId);
            return View(vehiculo);
        }

        // ===========================
        // GET: Vehiculos/Delete/5 (Confirmación)
        // ===========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vehiculo = await _vehiculoRepositorio.GetByIdAsync(id.Value);
            if (vehiculo == null) return NotFound();

            return View(vehiculo);
        }

        // ===========================
        // POST: Vehiculos/Delete/5 (Eliminar)
        // ===========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehiculoRepositorio.DeleteAsync(id);
            TempData["Mensaje"] = "Vehículo eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // DROPDOWN DE CLIENTES
        // ===========================
        private async Task PopulateClientesDropDownList(object? selectedCliente = null)
        {
            // Asumiendo que esta función está definida en VehiculoRepositorio y trae Clientes
            var clientes = await _vehiculoRepositorio.GetAllClientesAsync();
            
            // Usando "Id" y "NombreCompleto" de tu modelo Clientes
            ViewBag.ClienteId = new SelectList(clientes, "Id", "NombreCompleto", selectedCliente);
        }
    }
}