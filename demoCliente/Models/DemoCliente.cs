namespace demoCliente.Models
{
    public class DemoCliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<DemoDireccion> DemoDireccions { get; set; } = new List<DemoDireccion>();
    }
}
