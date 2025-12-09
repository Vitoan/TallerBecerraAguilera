using Microsoft.EntityFrameworkCore;
using TallerBecerraAguilera.Models;

namespace TallerBecerraAguilera.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

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

            // === TODAS TUS CONFIGURACIONES ANTERIORES (ToTable, HasKey, etc.) ===
            modelBuilder.Entity<Clientes>().ToTable("clientes");
            modelBuilder.Entity<Vehiculos>().ToTable("vehiculos");
            modelBuilder.Entity<Empleados>().ToTable("empleados");
            modelBuilder.Entity<Usuarios>().ToTable("usuarios");
            modelBuilder.Entity<Herramientas>().ToTable("herramientas");
            modelBuilder.Entity<Proveedores>().ToTable("proveedores");
            modelBuilder.Entity<Repuestos>().ToTable("repuestos");
            modelBuilder.Entity<OrdenesTrabajo>().ToTable("ordenes_trabajo");
            modelBuilder.Entity<PedidosRepuestos>().ToTable("pedidos_repuestos");
            modelBuilder.Entity<PedidoRepuestos>().ToTable("pedido_repuestos");
            modelBuilder.Entity<OtRepuestos>().ToTable("ot_repuestos");
            modelBuilder.Entity<OtHerramientas>().ToTable("ot_herramientas");

            modelBuilder.Entity<PedidoRepuestos>()
                .HasKey(pr => new { pr.PedidoId, pr.RepuestoId });

            modelBuilder.Entity<OtRepuestos>()
                .HasKey(or => new { or.OtId, or.RepuestoId });

            modelBuilder.Entity<OtHerramientas>()
                .HasKey(oh => new { oh.OtId, oh.HerramientaId });

            modelBuilder.Entity<Vehiculos>()
                .HasIndex(v => v.Patente)
                .IsUnique();

            modelBuilder.Entity<PedidosRepuestos>()
                .Property(p => p.Estado)
                .HasConversion<string>();

            // FIX DEFINITIVO PARA EL ENUM ESTADO DE ORDENES_TRABAJO
            modelBuilder.Entity<OrdenesTrabajo>(entity =>
            {
                entity.Property(e => e.Estado)
                      .HasColumnName("Estado")
                      .HasColumnType("int")
                      .HasConversion(
                          v => (int)v,
                      v => (EstadoOrden)v
                    );
            });
        }
    }
}