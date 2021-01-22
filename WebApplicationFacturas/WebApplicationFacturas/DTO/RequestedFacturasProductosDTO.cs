using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class RequestedFacturasProductosDTO
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public int ProductosId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }
    }
}
