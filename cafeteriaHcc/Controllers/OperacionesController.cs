using Microsoft.AspNetCore.Mvc;
using cafeteriaHcc.Models;
using cafeteriaHcc.Data;

namespace cafeteriaHcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionesController : ControllerBase
    {
        [HttpPost("agregar")]
        public IActionResult AgregarOrden([FromBody] ordenModel orden)
        {
            var respuesta = operacionesData.Agregar(orden);
            return Ok(respuesta);
        }

        [HttpPut("actualizar-estatus")]
        public IActionResult ActualizarEstatus([FromBody] ordenDetallesModel ordenDetalle)
        {
            var respuesta = operacionesData.actualizarEstatus(ordenDetalle);
            return Ok(respuesta);
        }

        [HttpPut("actualizar-producto")]
        public IActionResult ActualizarProducto([FromBody] ordenDetallesModel ordenDetalle)
        {
            var respuesta = operacionesData.actualizarProducto(ordenDetalle);
            return Ok(respuesta);
        }

        [HttpGet("listar-mesas-disponibles")]
        public IActionResult ListarMesasDisponibles()
        {
            var respuesta = operacionesData.ListarMesasDisponibles();
            return Ok(respuesta);
        }

        [HttpGet("listar-ordenes-activas")]
        public IActionResult ListarOrdenesActivas()
        {
            var respuesta = operacionesData.ListarOrdenesActivas();
            return Ok(respuesta);
        }

        [HttpDelete("eliminar-orden/{ordId}")]
        public IActionResult EliminarOrden(int ordId)
        {
            var respuesta = operacionesData.EliminarOrden(ordId);
            return Ok(respuesta);
        }
    }
}
