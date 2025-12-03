using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Data;


namespace TallerBecerraAguilera.Repositorios
{
    public class RepositorioBase<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositorioBase(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<List<T>> ListarTodo() => await _dbSet.ToListAsync();
        public async Task<T?> ObtenerPorId(int id) => await _dbSet.FindAsync(id);
        public async Task Agregar(T entidad) => await _dbSet.AddAsync(entidad);
        public void Actualizar(T entidad) => _dbSet.Update(entidad);
        public void Eliminar(T entidad) => _dbSet.Remove(entidad);
        public async Task<bool> Guardar() => await _context.SaveChangesAsync() > 0;
    }
}