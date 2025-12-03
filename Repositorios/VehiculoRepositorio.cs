using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;
using TallerBecerraAguilera.Models;

// ¡Notar que no implementa ninguna interfaz!
namespace TallerBecerraAguilera.Repositorios
{
    public class VehiculoRepositorio 
    {
        private readonly ApplicationDbContext _context;

        public VehiculoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los vehículos con el cliente asociado
        public async Task<IEnumerable<Vehiculos>> GetAllAsync()
        {
            return await _context.Vehiculos
                .Include(v => v.Cliente)
                .ToListAsync();
        }

        // Obtener un vehículo por ID
        public async Task<Vehiculos?> GetByIdAsync(int id)
        {
            return await _context.Vehiculos
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        // Obtener un vehículo por patente (útil para validaciones)
        public async Task<Vehiculos?> GetByPatenteAsync(string patente)
        {
            return await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Patente == patente);
        }

        // Agregar un nuevo vehículo
        public async Task AddAsync(Vehiculos vehiculo)
        {
            _context.Add(vehiculo);
            await _context.SaveChangesAsync();
        }

        // Actualizar un vehículo
        public async Task UpdateAsync(Vehiculos vehiculo)
        {
            _context.Update(vehiculo);
            await _context.SaveChangesAsync();
        }

        // Eliminar un vehículo
        public async Task DeleteAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Vehiculos.AnyAsync(v => v.Id == id);
        }

        // Obtener todos los clientes (necesario para el DropDownList en Create/Edit)
        public async Task<IEnumerable<Clientes>> GetAllClientesAsync()
        {
            return await _context.Clientes.ToListAsync();
        }
    }
}