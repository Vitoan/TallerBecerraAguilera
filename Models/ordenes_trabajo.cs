using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // <-- ¡Asegúrate de tener este using!

namespace TallerBecerraAguilera.Models
{
    public enum EstadoOrden
    {
        Pendiente = 0,
        EnReparacion = 1,
        Finalizada = 2,
        Entregada = 3
    }

    public class OrdenesTrabajo
    {
        [Key]
        public int Id { get; set; }

        // Mapeo crucial para el error:
        [Required]
        [Column("descripcion_falla")] // <-- Corregido: DescripcionFalla -> descripcion_falla
        public string DescripcionFalla { get; set; } = string.Empty;

        [Column("fecha_ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Column("fecha_estimada_entrega")]
        public DateTime? FechaEstimadaEntrega { get; set; }

        [Required]
        public EstadoOrden Estado { get; set; } // El enum se mapea como INT en la DB

        [Column("horas_estimadas", TypeName = "decimal(5, 2)")]
        public decimal? HorasEstimadas { get; set; }


        // Claves Foráneas
        [Column("vehiculo_id")]
        public int VehiculoId { get; set; }

        [Column("empleado_id")]
        public int EmpleadoId { get; set; }

        // Propiedades de Navegación
        public Vehiculos? Vehiculo { get; set; }
        public Empleados? Empleado { get; set; }


        // Fechas
        [Column("created_at")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;
    }
}