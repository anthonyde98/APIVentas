using APIVentas.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Vendedores
{
    public class Vendedor : IVendedor
    {
        public readonly VentasContext DbContext;

        public Vendedor(VentasContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
