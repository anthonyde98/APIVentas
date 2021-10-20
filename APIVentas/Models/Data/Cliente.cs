using System;
using System.Collections.Generic;

#nullable disable

namespace APIVentas.Models.Data
{
    public partial class Cliente
    {
        public Cliente()
        {
            OrdenVenta = new HashSet<OrdenVenta>();
        }

        public int ClienteId { get; set; }
        public string ClienteCodigo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public string NumeroTelefono { get; set; }
        public string Direccion { get; set; }
        public string MembresiaCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public virtual ICollection<OrdenVenta> OrdenVenta { get; set; }
    }
}
