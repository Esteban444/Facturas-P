using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.Models
{
    public class Facturas
    {
        public int Id { get; set;}
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string Estado { get; set; }
        public int Total { get; set; }

        public Clientes Cliente { get; set; }
        public Empleados Empleado { get; set; }
        public  ICollection<FacturasProductos> FacturasProductos { get; set; }
    }
}
