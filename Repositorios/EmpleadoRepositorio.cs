using TallerBecerraAguilera.Models;
using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;

namespace TallerBecerraAguilera.Repositorios
{
    public class EmpleadoRepositorio
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleados>> GetAllAsync()
        {
            return await _context.Empleados
                                 .OrderBy(e => e.Nombre)
                                 .ThenBy(e => e.Apellido)
                                 .ToListAsync();
        }

        public async Task<Empleados?> GetByIdAsync(int id)
        {
            return await _context.Empleados.FindAsync(id);
        }

        public async Task<Empleados?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Empleados.FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);
        }

        public async Task AddAsync(Empleados empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Empleados empleado)
        {
            _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
            }
        }
    }
}
