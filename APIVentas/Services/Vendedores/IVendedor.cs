using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Vendedores
{
    public interface IVendedor
    {
        Task<List<Models.DTOs.Vendedor.OutputVendedorDTO>> Buscar(int cantidad, int pagina);
        Task<bool> Existe(string codigo);
        Task<Models.DTOs.Vendedor.OutputVendedorDTO> Buscar(string codigo);
        Task<string> CrearCodigo(string nombre, string apellido);
        Task<Models.DTOs.Vendedor.InputVendedorDTO> Crear(Models.DTOs.Vendedor.InputVendedorDTO nuevoVendedor);
        Task<Models.DTOs.Vendedor.OutputVendedorDTO> Editar(string codigo, Models.DTOs.Vendedor.InputVendedorDTO nuevoInfoVendedor);
        Task<string> Eliminar(string codigo);
    }
}
