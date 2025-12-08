using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
// Nota: Puedes necesitar un using para el helper de DisplayName si lo usas en Details
// using TallerBecerraAguilera.Helpers; 

namespace TallerBecerraAguilera.Models
{
    // El enum EstadoOrden se mapeará a INT en la base de datos
    public enum EstadoOrden
    {
        [Display(Name = "0 - Pendiente (Ingreso)")]
        Pendiente = 0,
        [Display(Name = "1 - En Reparación")]
        EnReparacion = 1,
        [Display(Name = "2 - Finalizada (Esperando Retiro)")]
        Finalizada = 2,
        [Display(Name = "3 - Entregada (Cerrada)")]
        Entregada = 3
    }

    public class OrdenesTrabajo
    {
        [Key]
        [Display(Name = "OT #")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción de la falla es obligatoria.")]
        [Column("descripcion_falla")] 
        [Display(Name = "Descripción de la Falla")]
        public string DescripcionFalla { get; set; } = string.Empty;

        [Column("fecha_ingreso")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Column("fecha_estimada_entrega")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Estimada de Entrega")]
        public DateTime? FechaEstimadaEntrega { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Display(Name = "Estado")]
        public EstadoOrden Estado { get; set; }

        [Column("horas_estimadas", TypeName = "decimal(5, 2)")]
        [Display(Name = "Horas Est. de Trabajo")]
        [Range(0.0, 999.99, ErrorMessage = "El valor debe estar entre 0.00 y 999.99")]
        public decimal? HorasEstimadas { get; set; }

        // --- Claves Foráneas ---

        [Column("vehiculo_id")]
        [Display(Name = "Vehículo")]
        [Required(ErrorMessage = "Debe seleccionar un vehículo.")]
        public int VehiculoId { get; set; }

        [Column("empleado_id")]
        [Display(Name = "Empleado Asignado")]
        [Required(ErrorMessage = "Debe asignar un empleado.")]
        public int EmpleadoId { get; set; }

        // --- Propiedades de Navegación ---
        public Vehiculos? Vehiculo { get; set; }
        public Empleados? Empleado { get; set; }
        
        // Propiedades de auditoría
        [Column("created_at")]
        public DateTime? Created_at { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime? Updated_at { get; set; } = DateTime.Now;
    }
}