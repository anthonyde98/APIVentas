using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Productos.ProductoCategorias
{
    public interface IProductoCategoria
    {
        Task<List<Models.DTOs.Producto.ProductoCategoria.OutputProductoCategoriaDTO>> Buscar();
        Task<Models.DTOs.Producto.ProductoCategoria.OutputProductoCategoriaDTO> Buscar(string codigo);
        Task<bool> Existe(string codigo);
        Task<string> CrearCodigo(string nombre);
        Task<Models.DTOs.Producto.ProductoCategoria.InputProductoCategoriaDTO> Crear(Models.DTOs.Producto.ProductoCategoria.InputProductoCategoriaDTO nuevaCategoria);
        Task<Models.DTOs.Producto.ProductoCategoria.OutputProductoCategoriaDTO> Editar(string codigo, Models.DTOs.Producto.ProductoCategoria.InputProductoCategoriaDTO nuevaInfoCategoria);
        Task<string> Eliminar(string codigo);
        Task<List<Models.DTOs.Producto.ProductoCategoria.DropDownCategoriaDTO>> BuscarDropDown();
    }
}
