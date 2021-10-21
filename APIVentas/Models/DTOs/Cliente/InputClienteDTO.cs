using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Cliente
{
    public class InputClienteDTO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public string NumeroTelefono { get; set; }
        public string Direccion { get; set; }
    }
}
