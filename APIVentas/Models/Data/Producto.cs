using System;
using System.Collections.Generic;

#nullable disable

namespace APIVentas.Models.Data
{
    public partial class Producto
    {
        public Producto()
        {
            DetalleOrdenVenta = new HashSet<DetalleOrdenVenta>();
        }

        public int ProductoId { get; set; }
        public string ProductoCodigo { get; set; }
        public decimal PrecioEstandar { get; set; }
        public decimal PrecioLista { get; set; }
        public decimal Ganancia { get; set; }
        public decimal Tamanio { get; set; }
        public decimal Peso { get; set; }
        public decimal? Descuento { get; set; }
        public int CategoriaId { get; set; }
        public DateTime FechaInicioVenta { get; set; }
        public DateTime FechaFinVenta { get; set; }
        public string Empresa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public virtual ProductoCategoria Categoria { get; set; }
        public virtual ICollection<DetalleOrdenVenta> DetalleOrdenVenta { get; set; }
    }
}
