using System;
using System.Collections.Generic;

#nullable disable

namespace APIVentas.Models.Data
{
    public partial class DetalleOrdenVenta
    {
        public int DetalleOrdenVentaId { get; set; }
        public string DetalleOrdenVentaCodigo { get; set; }
        public int ProductoId { get; set; }
        public int ProductoCantidad { get; set; }
        public decimal? DescuentoTotal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
