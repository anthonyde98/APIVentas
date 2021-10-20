using APIVentas.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Productos
{
    public class Producto : IProducto
    {
        public readonly VentasContext DbContext;

        public Producto(VentasContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
