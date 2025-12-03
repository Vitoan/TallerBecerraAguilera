using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TallerBecerraAguilera.Models;
using TallerBecerraAguilera.Repositorios; // Asegúrate de incluir el namespace

namespace TallerBecerraAguilera.Controllers
{
    public class VehiculosController : Controller
    {
        // Inyectamos la clase concreta, no la interfaz
        private readonly VehiculoRepositorio _vehiculoRepositorio;

        public VehiculosController(VehiculoRepositorio vehiculoRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
        }

        // GET: Vehiculos
        public async Task<IActionResult> Index()
        {
            var vehiculos = await _vehiculoRepositorio.GetAllAsync();
            return View(vehiculos);
        }

        // GET: Vehiculos/Create
        public async Task<IActionResult> Create()
        {
            await PopulateClientesDropDownList();
            return View();
        }

        // POST: Vehiculos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculos vehiculo)
        {
            // Validación para evitar patentes duplicadas antes de guardar
            if (await _vehiculoRepositorio.GetByPatenteAsync(vehiculo.Patente) != null)
            {
                ModelState.AddModelError("Patente", "La patente ya está registrada en el sistema.");
            }

            if (ModelState.IsValid)
            {
                await _vehiculoRepositorio.AddAsync(vehiculo);
                TempData["success"] = "Vehículo creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            
            await PopulateClientesDropDownList(vehiculo.ClienteId);
            return View(vehiculo);
        }

        // Método privado para llenar el DropDownList de Clientes
        private async Task PopulateClientesDropDownList(object? selectedCliente = null)
        {
            var clientes = await _vehiculoRepositorio.GetAllClientesAsync();
            // Asegúrate de que el modelo Clientes tenga la propiedad NombreCompleto
            ViewBag.ClienteId = new SelectList(clientes, "Id", "NombreCompleto", selectedCliente);
        }

        // TODO: Agregar los métodos de Edit, Details y Delete
    }
}