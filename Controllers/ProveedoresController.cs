using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Helpers; // Necesario para DbUpdateConcurrencyException

namespace TallerBecerraAguilera.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProveedoresController : Controller
    {
        private readonly ProveedorRepositorio _repo;

        public ProveedoresController(ProveedorRepositorio repo)
        {
            _repo = repo;
        }

        // GET: Proveedores/Index (LISTAR TODOS)
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 10;

            var query = _repo.Query()
                             .OrderBy(p => p.Nombre);
            
            var paginated = await PaginatedList<Proveedores>.CreateAsync(query, pageNumber, pageSize);

            return View(paginated);
        }
        
        // GET: Proveedores/Details/5 (VISTA DETALLES)
        public async Task<IActionResult> Details(int id)
        {
            var proveedor = await _repo.GetByIdAsync(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        // GET: Proveedores/Create (VISTA CREAR)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create (ACCIÓN CREAR)
        [HttpPost]
        [ValidateAntiForgeryToken] // Seguridad
        public async Task<IActionResult> Create([Bind("Nombre,Contacto,Telefono,CondicionesCompra")] Proveedores proveedor)
        {
            // Validación de unicidad
            if (proveedor.Nombre != null && await _repo.GetByNameAsync(proveedor.Nombre) != null)
            {
                ModelState.AddModelError("Nombre", "Ya existe un proveedor con este nombre/razón social.");
            }
            
            if (ModelState.IsValid)
            {
                await _repo.AddAsync(proveedor);
                TempData["Mensaje"] = "Proveedor creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5 (VISTA EDITAR)
        public async Task<IActionResult> Edit(int id)
        {
            var proveedor = await _repo.GetByIdAsync(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        // POST: Proveedores/Edit (ACCIÓN ACTUALIZAR)
        [HttpPost]
        [ValidateAntiForgeryToken] // Seguridad
        // Se incluye Created_at en el Bind para asegurar que se mantenga el valor al actualizar.
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Contacto,Telefono,CondicionesCompra,Created_at")] Proveedores proveedor)
        {
            if (id != proveedor.Id) return NotFound();

            // Validación de unicidad en edición
            if (proveedor.Nombre != null)
            {
                var existingProveedor = await _repo.GetByNameAsync(proveedor.Nombre);
                if (existingProveedor != null && existingProveedor.Id != proveedor.Id)
                {
                    ModelState.AddModelError("Nombre", "El nombre/razón social ya está registrado para otro proveedor.");
                }
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.UpdateAsync(proveedor);
                    TempData["Mensaje"] = "Proveedor actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException) // Manejo de concurrencia
                {
                    if (await _repo.ExistsAsync(proveedor.Id) == false)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5 (VISTA ELIMINAR)
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _repo.GetByIdAsync(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5 (ACCIÓN ELIMINAR)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // Seguridad
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            TempData["Mensaje"] = "Proveedor eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}