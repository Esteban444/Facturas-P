using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.Models;

namespace WebApplicationFacturas.DTO
{
    public class FacturasProductosDTO
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public int ProductosId { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }

        public Facturas Facturas { get; set; }
        public Productos Productos { get; set; }
    }
}
