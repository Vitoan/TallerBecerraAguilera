using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;
// Asegúrate de que todos los modelos (Usuarios, Herramientas, OtRepuestos, etc.) estén definidos en la carpeta Models.

namespace TallerBecerraAguilera.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Aquí van TODAS tus tablas
        public DbSet<Clientes> Clientes { get; set; } = null!;
        public DbSet<Vehiculos> Vehiculos { get; set; } = null!;
        public DbSet<Empleados> Empleados { get; set; } = null!;
        public DbSet<Usuarios> Usuarios { get; set; } = null!;
        public DbSet<Herramientas> Herramientas { get; set; } = null!;
        public DbSet<Proveedores> Proveedores { get; set; } = null!;
        public DbSet<Repuestos> Repuestos { get; set; } = null!;
        public DbSet<OrdenesTrabajo> OrdenesTrabajo { get; set; } = null!;
        public DbSet<PedidosRepuestos> PedidosRepuestos { get; set; } = null!;
        public DbSet<PedidoRepuestos> PedidoRepuestos { get; set; } = null!;
        public DbSet<OtRepuestos> OtRepuestos { get; set; } = null!;
        public DbSet<OtHerramientas> OtHerramientas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----------------------------------------------------------------
            // 💡 SOLUCIÓN CLAVE: Mapeo de Entidades a Tablas en snake_case
            // ----------------------------------------------------------------
            
            modelBuilder.Entity<Clientes>().ToTable("clientes");
            modelBuilder.Entity<Vehiculos>().ToTable("vehiculos");
            modelBuilder.Entity<Empleados>().ToTable("empleados");
            modelBuilder.Entity<Usuarios>().ToTable("usuarios");
            modelBuilder.Entity<Herramientas>().ToTable("herramientas");
            modelBuilder.Entity<Proveedores>().ToTable("proveedores");
            modelBuilder.Entity<Repuestos>().ToTable("repuestos");
            
            // ESTA LÍNEA ES LA QUE CORRIGE EL ERROR DE ORDENES DE TRABAJO
            modelBuilder.Entity<OrdenesTrabajo>().ToTable("ordenes_trabajo");
            
            modelBuilder.Entity<PedidosRepuestos>().ToTable("pedidos_repuestos");
            modelBuilder.Entity<PedidoRepuestos>().ToTable("pedido_repuestos");
            modelBuilder.Entity<OtRepuestos>().ToTable("ot_repuestos");
            modelBuilder.Entity<OtHerramientas>().ToTable("ot_herramientas");


            // ----------------------------------------------------------------
            // 🔑 Definición de Claves Compuestas (M:N)
            // ----------------------------------------------------------------

            modelBuilder.Entity<PedidoRepuestos>()
                .HasKey(pr => new { pr.PedidoId, pr.RepuestoId });

            modelBuilder.Entity<OtRepuestos>()
                .HasKey(or => new { or.OtId, or.RepuestoId });

            modelBuilder.Entity<OtHerramientas>()
                .HasKey(oh => new { oh.OtId, oh.HerramientaId });


            // ----------------------------------------------------------------
            // 🔒 Definición de Índices (Opcional, pero bueno si no están en el modelo)
            // ----------------------------------------------------------------
            
            modelBuilder.Entity<Vehiculos>()
                .HasIndex(v => v.Patente)
                .IsUnique();

            // Puedes añadir más configuraciones aquí si es necesario, 
            // como relaciones o índices adicionales.
        }
    }
}