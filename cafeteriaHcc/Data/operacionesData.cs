using Microsoft.Data.SqlClient;
using System.Data;
using cafeteriaHcc.Models;

namespace cafeteriaHcc.Data
{
    public class operacionesData
    {
        // Devuelve las excepciones y respuestas correctas de mesas
        private static object GenerarRespuestaJsonMesas(int estatus, string mensaje, int codigo, List<mesasModel>? listado)
        {
            return new
            {
                estatus,
                mensaje,
                codigo,
                listado
            };
        }

        // Devuelve las excepciones y respuestas ordenes
        private static object GenerarRespuestaJsonOrdenes(int estatus, string mensaje, int codigo, List<ordenModel>? listado)
        {
            return new
            {
                estatus,
                mensaje,
                codigo,
                listado
            };
        }

        // Devuelve las excepciones y respuestas correctas
        private static object GenerarRespuestaJson(int estatus, string mensaje, int codigo)
        {
            return new
            {
                estatus,
                mensaje,
                codigo
            };
        }

        // Insertar una nueva orden 
        public static object Agregar(ordenModel orden)
        {
            using (SqlConnection conexion = new SqlConnection(conexiónBaseDeDatos.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("pa_agregar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ordId", orden.ord_id);
                cmd.Parameters.AddWithValue("@mesId", orden.mes_id);
                cmd.Parameters.AddWithValue("@catOrdId", orden.catord_id);
                cmd.Parameters.AddWithValue("@ordFechaInicio", orden.ord_fecha_inicio);
                cmd.Parameters.AddWithValue("@ordEstatus", orden.ord_estatus);

                try
                {
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return GenerarRespuestaJson(200, "Orden agregada correctamente", 1);
                }
                catch (Exception ex)
                {
                    return GenerarRespuestaJson(500, $"Error al agregar la orden: {ex.Message}", -1);
                }
            }
        }

        // Actualizar orden (Cambiar estatus) 
        public static object actualizarEstatus(ordenDetallesModel ordenDetalleOrden)
        {
            using (SqlConnection conexion = new SqlConnection(conexiónBaseDeDatos.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("pa_actualizarOrden", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ordId", ordenDetalleOrden.ord_id);
                cmd.Parameters.AddWithValue("@orddetEstatus", ordenDetalleOrden.orddet_estatus);

                try
                {
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return GenerarRespuestaJson(200, "Orden actualizada correctamente", 1);
                }
                catch (Exception ex)
                {
                    return GenerarRespuestaJson(500, $"Error al actualizar la orden: {ex.Message}", -1);
                }
            }
        }

        // Actualizar orden (Agregar nuevo producto) 
        public static object actualizarProducto(ordenDetallesModel ordenDetalleOrden)
        {
            using (SqlConnection conexion = new SqlConnection(conexiónBaseDeDatos.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("pa_actualizarOrden", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@orddetId", ordenDetalleOrden.orddet_id);
                cmd.Parameters.AddWithValue("@ordId", ordenDetalleOrden.ord_id);
                cmd.Parameters.AddWithValue("@proId", ordenDetalleOrden.pro_id);

                try
                {
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return GenerarRespuestaJson(200, "Orden actualizada correctamente", 1);
                }
                catch (Exception ex)
                {
                    return GenerarRespuestaJson(500, $"Error al actualizar la orden: {ex.Message}", -1);
                }
            }
        }

        // Obtener el número total de mesas disponibles y la cantidad de lugares por mesa.
        public static object ListarMesasDisponibles()
        {
            List<mesasModel> mesasDisponibles = new List<mesasModel>();
            using (SqlConnection conexion = new SqlConnection(conexiónBaseDeDatos.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("pa_obtenerMesasTotales", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int estatus = Convert.ToInt32(dr["mes_estatus"]);
                            if (estatus == 1)
                            {
                                mesasDisponibles.Add(new mesasModel()
                                {
                                    mes_id = Convert.ToInt32(dr["mes_id"]),
                                    mes_lugares = Convert.ToInt32(dr["mes_lugares"]),
                                    mes_disponible = Convert.ToInt32(dr["mes_disponible"]),
                                    mes_estatus = estatus
                                });
                            }
                        }
                    }
                    return GenerarRespuestaJsonMesas(200, "Mesas disponibles listadas correctamente", 1, mesasDisponibles);
                }
                catch (Exception ex)
                {
                    return GenerarRespuestaJson(500, $"Error al listar las mesas disponibles: {ex.Message}", -1);
                }
            }
        }

        // Obtener el número total de órdenes y en que mesa están.
        public static object ListarOrdenesActivas()
        {
            List<ordenModel> ordenesActivas = new List<ordenModel>();
            using (SqlConnection conexion = new SqlConnection(conexiónBaseDeDatos.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("pa_obtenerOrdenesTotales", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int estatus = Convert.ToInt32(dr["ord_status"]);
                            if (estatus == 1)
                            {
                                ordenesActivas.Add(new ordenModel()
                                {
                                    ord_id = Convert.ToInt32(dr["ord_id"]),
                                    mes_id = Convert.ToInt32(dr["mes_id"]),
                                    catord_id = Convert.ToInt32(dr["catord_id"]),
                                    ord_fecha_inicio = Convert.ToDateTime(dr["ord_fecha_inicio"].ToString()),
                                    ord_estatus = estatus
                                });
                            }
                        }
                    }
                    return GenerarRespuestaJsonOrdenes(200, "Órdenes activas listadas correctamente", 1, ordenesActivas);
                }
                catch (Exception ex)
                {
                    return GenerarRespuestaJson(500, $"Error al listar las órdenes activas: {ex.Message}", -1);
                }
            }
        }

        // Eliminar orden(borrado lógico)
        public static object EliminarOrden(int ordId)
        {
            using (SqlConnection conexion = new SqlConnection(conexiónBaseDeDatos.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("pa_obtenerOrdenPorId", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ordId", ordId);

                try
                {
                    conexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            int estatus = Convert.ToInt32(dr["ord_status"]);
                            if (estatus != 0)
                            {
                                return GenerarRespuestaJson(500, "No se puede eliminar la orden porque el estado no es 0", -1);
                            }
                        }
                        else
                        {
                            return GenerarRespuestaJson(500, "Orden no encontrada", -1);
                        }
                    }

                    // Si el estado es 0, procedemos a eliminar
                    cmd = new SqlCommand("pa_eliminarOrden", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ordId", ordId);
                    cmd.ExecuteNonQuery();

                    return GenerarRespuestaJson(200, "Orden eliminada correctamente", 1);
                }
                catch (Exception ex)
                {
                    return GenerarRespuestaJson(500, $"Error al eliminar la orden: {ex.Message}", -1);
                }
            }
        }

    }
}
