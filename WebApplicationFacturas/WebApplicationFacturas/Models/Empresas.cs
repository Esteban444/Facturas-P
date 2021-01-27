using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.DTO;

namespace WebApplicationFacturas.Models
{
    public class Empresas
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Propietario { get; set; }
        public string Email { get; set; }
        public string Nit { get; set; }

        public  ICollection<Empleados> Empleados { get; set; }
    }
}
