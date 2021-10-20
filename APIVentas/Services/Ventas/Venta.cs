using APIVentas.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Ventas
{
    public class Venta :IVenta
    {
        public readonly VentasContext DbContext;

        public Venta(VentasContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
