using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class CreacionCategoriasDTO
    {
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

    }
}
