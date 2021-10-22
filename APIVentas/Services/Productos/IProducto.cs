using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Productos
{
    public interface IProducto
    {
        Task<List<Models.DTOs.Producto.OutputProductoDTO>> Buscar();
        Task<Models.DTOs.Producto.OutputProductoDTO> Buscar(string codigo);
        Task<bool> Existe(string codigo);
        Task<string> CrearCodigo(string nombre);
        Task<decimal> BuscarImpuesto(int id);
        Task<Models.DTOs.Producto.InputProductoDTO> Crear(Models.DTOs.Producto.InputProductoDTO nuevoProducto);
        Task<Models.DTOs.Producto.OutputProductoDTO> Editar(string codigo, Models.DTOs.Producto.InputProductoDTO nuevaInfoProducto);
        Task<string> Eliminar(string codigo);
    }
}
