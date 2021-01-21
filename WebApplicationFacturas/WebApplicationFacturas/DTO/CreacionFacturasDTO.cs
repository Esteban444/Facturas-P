using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class CreacionFacturasDTO
    {
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string Estado { get; set; }
        public int Total { get; set; }
    }
}
