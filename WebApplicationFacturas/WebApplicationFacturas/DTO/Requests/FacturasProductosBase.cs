using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO.Requests
{
    public class FacturasProductosBase
    {
        public int? FacturaId { get; set; }
        public int? ProductosId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public Decimal Precio { get; set; }
    }
}

