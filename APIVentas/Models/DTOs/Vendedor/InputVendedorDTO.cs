using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Models.DTOs.Vendedor
{
    public class InputVendedorDTO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public string NumeroTelefono { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public decimal SalarioBruto { get; set; }
        public string SeguroCodigo { get; set; }
        public string AfpCodigo { get; set; }
        public decimal DeduccionTotal { get; set; }
    }
}
