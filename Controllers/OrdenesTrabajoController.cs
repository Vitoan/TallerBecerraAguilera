using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization; // Necesario para Select

namespace TallerBecerraAguilera.Controllers
{
    public class OrdenesTrabajoController : Controller
    {
        private readonly OrdenTrabajoRepositorio _otRepositorio;

        public OrdenesTrabajoController(OrdenTrabajoRepositorio otRepositorio)
        {
            _otRepositorio = otRepositorio;
        }

        // ===========================
        // DROPDOWNS: Vehículos y Empleados
        // ===========================

        private async Task PopulateDropDownsAsync(object? selectedVehiculo = null, object? selectedEmpleado = null)
        {
            // Dropdown de Vehículos (Patente y Cliente)
            var vehiculos = await _otRepositorio.GetAllVehiculosAsync();
            var vehiculoList = vehiculos.Select(v => new
            {
                v.Id,
                // Formato: "Patente - Cliente Nombre Completo"
                DisplayName = $"{v.Patente} - {v.Cliente?.Nombre} {v.Cliente?.Apellido}" 
            }).ToList();
            
            ViewBag.VehiculoId = new SelectList(vehiculoList, "Id", "DisplayName", selectedVehiculo);
            
            // Dropdown de Empleados (Nombre Completo)
            var empleados = await _otRepositorio.GetAllEmpleadosAsync();
            var empleadoList = empleados.Select(e => new
            {
                e.Id,
                // Asumiendo que Empleados tiene Nombre y Apellido
                DisplayName = $"{e.Nombre} {e.Apellido}" 
            }).ToList();
            
            ViewBag.EmpleadoId = new SelectList(empleadoList, "Id", "DisplayName", selectedEmpleado);
        }

        // ===========================
        // CRUD ACTIONS
        // ===========================
        
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 1; // Cantidad de órdenes por página (puedes cambiarlo a 10)
            
            // Llamamos al nuevo método del repositorio
            var ots = await _otRepositorio.GetAllPaginatedAsync(pageNumber ?? 1, pageSize);
            
            return View(ots);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var ot = await _otRepositorio.GetByIdAsync(id.Value); 
            if (ot == null) return NotFound();
            return View(ot);
        }
        
        public async Task<IActionResult> Create()
        {
            // Prepara los SelectLists para la vista
            await PopulateDropDownsAsync();
            // Pre-llena la fecha de ingreso y el estado
            var ot = new OrdenesTrabajo { 
                FechaIngreso = DateTime.Today,
                Estado = EstadoOrden.Pendiente 
            };
            return View(ot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehiculoId,EmpleadoId,DescripcionFalla,FechaIngreso,FechaEstimadaEntrega,Estado,HorasEstimadas")] OrdenesTrabajo ot)
        {
            if (ModelState.IsValid)
            {
                await _otRepositorio.AddAsync(ot);
                TempData["Mensaje"] = $"Orden de Trabajo #{ot.Id} creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            
            await PopulateDropDownsAsync(ot.VehiculoId, ot.EmpleadoId);
            return View(ot);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var ot = await _otRepositorio.GetByIdAsync(id.Value);
            if (ot == null) return NotFound();
            
            await PopulateDropDownsAsync(ot.VehiculoId, ot.EmpleadoId);
            return View(ot);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Incluimos Created_at para mantener la fecha de creación original
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehiculoId,EmpleadoId,DescripcionFalla,FechaIngreso,FechaEstimadaEntrega,Estado,HorasEstimadas,Created_at")] OrdenesTrabajo ot)
        {
            if (id != ot.Id) return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _otRepositorio.UpdateAsync(ot);
                    TempData["Mensaje"] = $"Orden de Trabajo #{ot.Id} actualizada exitosamente.";
                }
                catch (DbUpdateConcurrencyException) 
                {
                    if (await _otRepositorio.ExistsAsync(ot.Id) == false)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            await PopulateDropDownsAsync(ot.VehiculoId, ot.EmpleadoId);
            return View(ot);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var ot = await _otRepositorio.GetByIdAsync(id.Value);
            if (ot == null) return NotFound();
            return View(ot);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _otRepositorio.DeleteAsync(id);
            TempData["Mensaje"] = "Orden de Trabajo eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> MisOTs()
        {
            var userId = int.Parse(User.FindFirst("Id")!.Value);

            var ots = await _otRepositorio.GetByEmpleadoAsync(userId);

            return View(ots);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> CambiarEstado(int id, int nuevoEstado)
        {
            var ot = await _otRepositorio.GetByIdAsync(id);
            if (ot == null) return NotFound();

            var userId = int.Parse(User.FindFirst("Id")!.Value);
            if (ot.EmpleadoId != userId) return Forbid();

            ot.Estado = (EstadoOrden)nuevoEstado;
            await _otRepositorio.UpdateAsync(ot);

            TempData["Mensaje"] = $"Estado de OT #{ot.Id} actualizado a {ot.Estado}";
            return RedirectToAction(nameof(MisOTs));
        }
    }
}