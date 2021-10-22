using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIVentas.Services.Clientes
{
    public interface ICliente
    {
        Task<List<Models.DTOs.Cliente.OutputClienteDTO>> Buscar();
        Task<bool> Existe(string codigo);
        Task<Models.DTOs.Cliente.OutputClienteDTO> Buscar(string codigo);
        Task<string> CrearCodigo(string nombre, string apellido);
        Task<string> CrearMembresiaCodigo();
        Task<Models.DTOs.Cliente.InputClienteDTO> Crear(Models.DTOs.Cliente.InputClienteDTO nuevoCliente);
        Task<Models.DTOs.Cliente.OutputClienteDTO> Editar(string codigo, Models.DTOs.Cliente.InputClienteDTO nuevoInfoCliente);
        Task<string> Eliminar(string codigo);
    }
}
