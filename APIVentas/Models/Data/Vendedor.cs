using System;
using System.Collections.Generic;

#nullable disable

namespace APIVentas.Models.Data
{
    public partial class Vendedor
    {
        public Vendedor()
        {
            OrdenVenta = new HashSet<OrdenVenta>();
        }

        public int VendedorId { get; set; }
        public string VendedorCodigo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public string NumeroTelefono { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }
        public string SeguroCodigo { get; set; }
        public string AfpCodigo { get; set; }
        public decimal DeduccionTotal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public virtual ICollection<OrdenVenta> OrdenVenta { get; set; }
    }
}
