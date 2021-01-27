using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO.Requests
{
    public class CargosBase
    {
        public string Nombre { get; set; }
        public int? EmpleadoId { get; set; }

        
    }
}
