using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendedoresController : ControllerBase
    {
        public readonly Services.Vendedores.IVendedor VendedorManager;
        public readonly Services.Ventas.IVenta VentaManager;

        public VendedoresController(Services.Vendedores.IVendedor vendedorManager,
                                    Services.Ventas.IVenta ventaManager)
        {
            VendedorManager = vendedorManager;
            VentaManager = ventaManager;
        }

        // GET: api/<VendedoresController>
        [HttpGet]                            //---------------------Paginacion-----------------------//
        public async Task<IActionResult> Get([FromQuery] int cantidad = 10, [FromQuery] int pagina = 1)
        {
            try
            {
                var Vendedores = await VendedorManager.Buscar(cantidad, pagina);

                return Ok(Vendedores);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<VendedoresController>/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(string codigo)
        {
            try
            {
                var respuesta = await VendedorManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Vendedor = await VendedorManager.Buscar(codigo);

                return Ok(Vendedor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<VendedoresController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.DTOs.Vendedor.InputVendedorDTO nuevoVendedor)
        {
            try
            {
                var Vendedor = await VendedorManager.Crear(nuevoVendedor);

                return Created("Vendedor", Vendedor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<VendedoresController>/5
        [HttpPut("{codigo}")]
        public async Task<IActionResult> Put(string codigo, [FromBody] Models.DTOs.Vendedor.InputVendedorDTO nuevaInfoVendedor)
        {
            try
            {
                var respuesta = await VendedorManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Vendedor = await VendedorManager.Editar(codigo, nuevaInfoVendedor);

                return Ok(Vendedor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<VendedoresController>/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            try
            {
                var respuesta = await VendedorManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var codigoVendedor = await VendedorManager.Eliminar(codigo);

                return Ok(new { message = "El vendedor " + codigoVendedor + " fue eliminado con exito." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //-------------------------------------------VENDEDOR ORDEN DE VENTA-------------------------------------------//
        //------------------------------------------------------------------------------------------------------------//

        [HttpGet("Ordenes/")]
        public async Task<IActionResult> GetOrdenes([FromHeader] string codigo)
        {
            try
            {
                var respuesta = await VentaManager.ExisteOrdenPorVendedor(codigo);

                if (!respuesta)
                    return NotFound();

                var Detalle = await VentaManager.BuscarOrdenPorVendedor(codigo);

                return Ok(Detalle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
