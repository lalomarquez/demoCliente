namespace demoCliente.Models
{
    public class DemoDireccion
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public string NumeroInterior { get; set; }
        public string Colonia { get; set; }
        public string CodigoPostal { get; set; }
        public string Municipio { get; set; }
        public int ClienteId { get; set; }
        public bool IsDeleted { get; set; }
        public DemoCliente Cliente { get; set; }
    }
}
