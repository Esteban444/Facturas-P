using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFacturas.Models
{
    public class Empleados
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public int EmpresaId { get; set; }
        

        public  Empresas Empresas { get; set; }
        public  ICollection<Cargos> Cargos { get; set; }
        public  ICollection<Facturas> Facturas { get; set; }
    }
}
