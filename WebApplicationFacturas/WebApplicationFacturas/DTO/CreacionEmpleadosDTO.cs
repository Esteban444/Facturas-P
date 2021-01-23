using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.DTO
{
    public class CreacionEmpleadosDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public int EmpresaId { get; set; }
    }
}
