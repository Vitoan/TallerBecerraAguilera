using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Repositorios;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Helpers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // Necesario para Select

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
            var vehiculos = await _otRepositorio.GetAllVehiculosAsync() ?? new List<Vehiculos>();
            var vehiculoList = vehiculos.Select(v => new
            {
                Id = v.id,
                // Formato: "Patente - Cliente Nombre Completo"
                DisplayName = $"{v.patente} - {v.Cliente?.Nombre} {v.Cliente?.Apellido}" 
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
        
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            EstadoOrden? estado = null,
            string? patente = null,
            string? empleado = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null)
        {
            int pageSize = 5;

            var query = _otRepositorio.Query();

            // ESTADO
            if (estado.HasValue)
            {
                query = query.Where(o => o.Estado == estado.Value);
            }

            // PATENTE
            if (!string.IsNullOrWhiteSpace(patente))
            {
                var pat = patente.Trim().ToLower();
                query = query.Where(o =>
                    o.Vehiculo != null &&
                    o.Vehiculo.patente.ToLower().Contains(pat));
            }

            // EMPLEADO (nombre o apellido, case-insensitive)
            if (!string.IsNullOrWhiteSpace(empleado))
            {
                var emp = empleado.Trim().ToLower();
                query = query.Where(o =>
                    o.Empleado != null &&
                    (
                        o.Empleado.Nombre.ToLower().Contains(emp) ||
                        o.Empleado.Apellido.ToLower().Contains(emp)
                    ));
            }

            // FECHA DESDE
            if (fechaDesde.HasValue)
            {
                query = query.Where(o => o.FechaIngreso.Date >= fechaDesde.Value.Date);
            }

            // FECHA HASTA
            if (fechaHasta.HasValue)
            {
                query = query.Where(o => o.FechaIngreso.Date <= fechaHasta.Value.Date);
            }

            // ORDEN
            query = query.OrderByDescending(o => o.FechaIngreso);

            // PAGINADO
            var paginated = await PaginatedList<OrdenesTrabajo>
                .CreateAsync(query, pageNumber, pageSize);

            // VIEWBAGS
            ViewBag.EstadoSeleccionado = estado;
            ViewBag.Patente = patente;
            ViewBag.Empleado = empleado;
            ViewBag.FechaDesde = fechaDesde?.ToString("yyyy-MM-dd");
            ViewBag.FechaHasta = fechaHasta?.ToString("yyyy-MM-dd");

            return View(paginated);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var ot = await _otRepositorio.GetByIdAsync(id.Value); 
            if (ot == null) return NotFound();
            return View(ot);
        }
        
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            // Pre-llena la fecha de ingreso y el estado
            var ot = new OrdenesTrabajo { 
                FechaIngreso = DateTime.Today,
                Estado = EstadoOrden.Pendiente 
            };
            return View(ot);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehiculoId,EmpleadoId,DescripcionFalla,FechaIngreso,FechaEstimadaEntrega,Estado,HorasEstimadas")] OrdenesTrabajo ot)
        {
            if (ModelState.IsValid)
            {
                await _otRepositorio.AddAsync(ot);
                TempData["Mensaje"] = $"Orden de Trabajo #{ot.Id} creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(ot);
        }
        
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ot = await _otRepositorio.GetByIdAsync(id.Value);
            if (ot == null) return NotFound();
            
            return View(ot);
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrador")]
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
            
            return View(ot);
        }
        
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var ot = await _otRepositorio.GetByIdAsync(id.Value);
            if (ot == null) return NotFound();
            return View(ot);
        }
        
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _otRepositorio.DeleteAsync(id);
            TempData["Mensaje"] = "Orden de Trabajo eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> MisOTs(int? pageNumber)
        {
            int pageSize = 5;

            var usuarioIdClaim = User.FindFirst("Id")?.Value;
            if (usuarioIdClaim == null)
                return Forbid();

            var usuarioId = int.Parse(usuarioIdClaim);

            var empleado = await _otRepositorio.GetEmpleadoByUserIdAsync(usuarioId);

            if (empleado == null)
                return View(PaginatedList<OrdenesTrabajo>.CreateEmpty(pageSize));

            var ots = await _otRepositorio.GetByEmpleadoPaginatedAsync(empleado.Id, pageNumber ?? 1, pageSize);

            return View(ots);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> CambiarEstado(int id, int nuevoEstado)
        {
            var ot = await _otRepositorio.GetByIdAsync(id);
            if (ot == null) return NotFound();

            var usuarioIdClaim = User.FindFirst("Id")?.Value;
            if (usuarioIdClaim == null) return Forbid();

            var usuarioId = int.Parse(usuarioIdClaim);

            var empleado = await _otRepositorio.GetEmpleadoByUserIdAsync(usuarioId);
            if (empleado == null) return Forbid();

            if (ot.EmpleadoId != empleado.Id)
                return Forbid();    

            ot.Estado = (EstadoOrden)nuevoEstado;
            await _otRepositorio.UpdateAsync(ot);

            TempData["Mensaje"] = $"Estado de OT #{ot.Id} actualizado a {ot.Estado}";
            return RedirectToAction(nameof(MisOTs));
        }

        [HttpGet]
        public async Task<IActionResult> BuscarVehiculos(string term)
        {
            var vehiculos = await _otRepositorio.GetAllVehiculosAsync();
            var results = vehiculos
                .Where(v => v.patente.Contains(term))
                .Select(v => new { id = v.id, text = $"{v.patente} - {v.Cliente?.Nombre} {v.Cliente?.Apellido}" })
                .ToList();
            return Json(new { results });
        }

        [HttpGet]
        public async Task<IActionResult> BuscarEmpleados(string term)
        {
            var empleados = await _otRepositorio.GetAllEmpleadosAsync();
            var results = empleados
                .Where(e => e.Nombre.Contains(term) || e.Apellido.Contains(term))
                .Select(e => new { id = e.Id, text = $"{e.Nombre} {e.Apellido}" })
                .ToList();
            return Json(new { results });
        }
    }
}