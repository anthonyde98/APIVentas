using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Ventas
{
    public interface IVenta
    {
        Task<string> CrearCodigo(char c);
        Task<Models.DTOs.Venta.InputOrdenVentaDTO> ProcesarOrden(Models.DTOs.Venta.InputOrdenVentaDTO orden);
        Task<List<Models.DTOs.Venta.OutputOrdenVentaDTO>> Buscar(int cantidad, int pagina);
        Task<Models.DTOs.Venta.OutputOrdenVentaDTO> Buscar(string codigo);
        Task<bool> ExisteOrdenPorCliente(string codigo);
        Task<List<Models.DTOs.Venta.OutputOrdenVentaDTO>> BuscarOrdenPorCliente(string codigo);
        Task<bool> ExisteOrdenPorVendedor(string codigo);
        Task<List<Models.DTOs.Venta.OutputOrdenVentaDTO>> BuscarOrdenPorVendedor(string codigo);
        Task<bool> Existe(string codigo);
        Task<string> Eliminar(string codigo);

    }
}
