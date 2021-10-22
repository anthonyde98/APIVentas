using System;
using System.Collections.Generic;

#nullable disable

namespace APIVentas.Models.Data
{
    public partial class OrdenVenta
    {
        public int OrdenVentaId { get; set; }
        public string OrdenVentaCodigo { get; set; }
        public DateTime OrdenFecha { get; set; }
        public decimal ValorCantidadNeto { get; set; }
        public decimal ValorCantidadBruto { get; set; }
        public decimal ValorPagado { get; set; }
        public decimal ValorDevuelto { get; set; }
        public decimal DescuentoTotalOrden { get; set; }
        public string VendedorCodigo { get; set; }
        public string ClienteCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public virtual Cliente ClienteCodigoNavigation { get; set; }
        public virtual Vendedor VendedorCodigoNavigation { get; set; }
    }
}
