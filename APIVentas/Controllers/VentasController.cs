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
    public class VentasController : ControllerBase
    {
        public readonly Services.Ventas.IVenta VentaManager;
        public readonly Services.Ventas.DetalleVentas.IDetalleVenta DetalleVentaManager;
        public readonly Services.Clientes.ICliente ClienteManager;
        public readonly Services.Vendedores.IVendedor VendedorManager;
        public readonly Services.Productos.IProducto ProductoManager;

        public VentasController(Services.Ventas.IVenta ventaManager,
                                Services.Ventas.DetalleVentas.IDetalleVenta detalleVentaManager,
                                Services.Clientes.ICliente clienteManager,
                                Services.Vendedores.IVendedor vendedorManager,
                                Services.Productos.IProducto productoManager)
        {
            VentaManager = ventaManager;
            DetalleVentaManager = detalleVentaManager;
            ClienteManager = clienteManager;
            VendedorManager = vendedorManager;
            ProductoManager = productoManager;

        }

        // GET: api/<VentasController>
        [HttpGet]                            //---------------------Paginacion-----------------------//
        public async Task<IActionResult> Get([FromQuery] int cantidad = 10, [FromQuery] int pagina = 1)
        {
            try
            {
                var Ventas = await VentaManager.Buscar(cantidad, pagina);

                return Ok(Ventas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<VentasController>/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(string codigo)
        {
            try
            {
                var respuesta = await VentaManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var Venta = await VentaManager.Buscar(codigo);

                return Ok(Venta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<VentasController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.DTOs.Venta.InputOrdenVentaDTO nuevaOrden)
        {
            try
            {
                var existeCliente = await ClienteManager.Existe(nuevaOrden.ClienteCodigo);
                var existeVendedor = await VendedorManager.Existe(nuevaOrden.VendedorCodigo);

                if (!existeCliente)
                    return NotFound(new { message = "El cliente con codigo " + nuevaOrden.ClienteCodigo + " no fue encontrado." });
                else if(!existeVendedor)
                    return NotFound(new { message = "El vendedor con codigo " + nuevaOrden.VendedorCodigo + " no fue encontrado." });

                foreach(var producto in nuevaOrden.Productos)
                {
                    var existeProducto = await ProductoManager.Existe(producto.ProductoCodigo);

                    if(!existeProducto)
                        return NotFound(new { message = "El producto con codigo " + producto.ProductoCodigo + " no fue encontrado." });
                }

                var Orden = await VentaManager.ProcesarOrden(nuevaOrden);

                return Created("OrdenVenta", Orden);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<VentasController>
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(string codigo)
        {
            try
            {
                var respuesta = await VentaManager.Existe(codigo);

                if (!respuesta)
                    return NotFound();

                var codigoVenta = await VentaManager.Eliminar(codigo);

                return Ok(new { message = "La orden de venta " + codigoVenta + " fue eliminada con exito." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //-------------------------------------------DETALLE ORDEN DE VENTA-------------------------------------------//
        //------------------------------------------------------------------------------------------------------------//
        [HttpGet]
        [Route("DetalleVentas/")]                       //---------------------Paginacion-----------------------//
        public async Task<IActionResult> GetAllDetalles([FromQuery] int cantidad = 10, [FromQuery] int pagina = 1)
        {
            try
            {
                var Detalles = await DetalleVentaManager.Buscar(cantidad, pagina);

                return Ok(Detalles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("DetalleVentas/{codigo}")]
        public async Task<IActionResult> GetDetalle(string codigo)
        {
            try
            {
                var respuesta = await DetalleVentaManager.Existe(codigo);

                if(!respuesta)
                    return NotFound();

                var Detalle = await DetalleVentaManager.Buscar(codigo);

                return Ok(Detalle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
