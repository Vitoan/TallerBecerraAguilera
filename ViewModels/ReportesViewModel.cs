namespace TallerBecerraAguilera.ViewModels
{
    public class ReportesViewModel
    {
        // Al poner '= Array.Empty<...>()' le decimos que arranca vacía, no nula.
        public string[] Meses { get; set; } = Array.Empty<string>();
        public decimal[] Totales { get; set; } = Array.Empty<decimal>();

        public string[] EstadosNombres { get; set; } = Array.Empty<string>();
        public int[] EstadosCantidades { get; set; } = Array.Empty<int>();
    }
}