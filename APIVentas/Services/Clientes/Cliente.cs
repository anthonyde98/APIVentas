using APIVentas.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Clientes
{
    public class Cliente : ICliente
    {
        public readonly VentasContext DbContext;

        public Cliente(VentasContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
