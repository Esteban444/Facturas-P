using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class FacturasDTO
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string Estado { get; set; }
        public int Total { get; set; }

        public ClientesDTO Cliente { get; set; }
        public EmpleadosDTO Empleado { get; set; }
        //public virtual ICollection<FacturasProductos> FacturasProductos { get; set; }
    }
}
