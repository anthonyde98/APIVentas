using System;
using System.Collections.Generic;

#nullable disable

namespace APIVentas.Models.Data
{
    public partial class ProductoCategoria
    {
        public ProductoCategoria()
        {
            Productos = new HashSet<Producto>();
        }

        public int CategoriaId { get; set; }
        public string CategoriaCodigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Impuesto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
