using Microsoft.AspNetCore.Mvc;
using TallerBecerraAguilera.Repositorio; // Usando el repositorio sin interfaz
using TallerBecerraAguilera.Models;
using System.Threading.Tasks;

namespace TallerBecerraAguilera.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteRepositorio _clienteRepositorio;

        public ClientesController(ClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        // GET: Clientes (LISTAR TODOS)
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRepositorio.GetAllAsync();
            return View(clientes);
        }

        // GET: Clientes/Create (VISTA CREAR)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create (ACCIÓN CREAR)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Dni,Telefono,Email")] Clientes cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteRepositorio.AddAsync(cliente);
                TempData["Mensaje"] = "Cliente creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5 (VISTA EDITAR)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: Clientes/Edit/5 (ACCIÓN ACTUALIZAR)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Dni,Telefono,Email,Created_at")] Clientes cliente)
        {
            if (id != cliente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Asegurar que Created_at y Updated_at se manejen correctamente si no están en el Bind
                // Por simplicidad, se deja Created_at para ser usado si está en el Bind.

                // Actualizar el registro
                await _clienteRepositorio.UpdateAsync(cliente);
                TempData["Mensaje"] = "Cliente actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        
        // GET: Clientes/Delete/5 (VISTA ELIMINAR)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: Clientes/Delete/5 (ACCIÓN ELIMINAR)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteRepositorio.DeleteAsync(id);
            TempData["Mensaje"] = "Cliente eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Details/5 (VISTA DETALLES)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _clienteRepositorio.GetByIdAsync(id.Value);
            if (cliente == null) return NotFound();
            return View(cliente);
        }
    }
}