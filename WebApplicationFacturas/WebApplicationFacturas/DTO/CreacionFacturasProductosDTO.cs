﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class CreacionFacturasProductosDTO
    {
        
        public int FacturaId { get; set; }
        public int ProductosId { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }
    }
}
