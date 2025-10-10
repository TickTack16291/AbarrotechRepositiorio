using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;


/// <summary>
/// Conexion a la base de datos y funciones para hacer consultas en ella
/// </summary>

namespace LoginDeAbarrotech
{
    internal class ConexionBD
    {
        private string conexionString =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=abarrotechv2;" +
            "Uid=root;" +
            "Pwd=;";

        public ConexionBD() {}

        /// <summary>
        /// Operaciones de productos en la base de datos
        /// </summary>
        public List<Producto> ObtenerProductos()
        {
            List<Producto> listaProductos = new List<Producto>();

            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"SELECT id_producto, nombre_producto, marca_producto, presentacion_producto, unidad_medida_producto, 
                                  precio_compra_producto, precio_venta_producto, estado_producto, categoria_producto, id_proveedor_producto
                           FROM productos";
                    using (var command = new MySqlCommand(sql, Conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto productoAux = new Producto(
                                reader.GetInt32(0),       // id_producto
                                reader.GetString(1),      // nombre_producto
                                reader.GetString(2),      // marca_producto
                                reader.GetString(3),      // presentacion_producto
                                reader.GetString(4),      // unidad_medida_producto
                                reader.GetFloat(5),       // precio_compra_producto
                                reader.GetFloat(6),       // precio_venta_producto
                                reader.GetInt32(7),       // estado_producto
                                reader.GetString(8),      // categoria_producto
                                reader.GetInt32(9)        // id_proveedor_producto
                            );

                            listaProductos.Add(productoAux);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                }
            }

            return listaProductos;
        }
        public bool AgregarProducto(Producto nuevoProducto)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO `productos` (`nombre_producto`, `marca_producto`, `presentacion_producto`, `unidad_medida_producto`,
                                  `estado_producto` ,`precio_venta_producto`, `precio_compra_producto`, `categoria_producto`) 
                                   VALUES(@nombre_producto, @marca_producto, @presentacion_producto, @unidad_medida_producto, @estado_producto,
                                   @precio_venta_producto, @precio_compra_producto, @categoria_producto);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        // Agregamos parámetros para evitar una inyecion de SQL
                        //command.Parameters.AddWithValue("@Id_Producto", nuevoProducto.idProducto); // Es autoincrementable
                        command.Parameters.AddWithValue("@nombre_producto", nuevoProducto.nombre_producto);
                        command.Parameters.AddWithValue("@marca_producto", nuevoProducto.marca_producto);
                        command.Parameters.AddWithValue("@presentacion_producto", nuevoProducto.presentacion_producto);
                        command.Parameters.AddWithValue("@unidad_medida_producto", nuevoProducto.unidad_medida_producto);
                        command.Parameters.AddWithValue("@estado_producto", 1); // Se colocara por defecto 1
                        command.Parameters.AddWithValue("@precio_venta_producto", nuevoProducto.precio_venta_producto);
                        command.Parameters.AddWithValue("@precio_compra_producto", nuevoProducto.precio_compra_producto);
                        command.Parameters.AddWithValue("@categoria_producto", nuevoProducto.categoria_producto);
                        //command.Parameters.AddWithValue("@id_proveedor_producto", nuevoProducto.idProveedor); // No tenemos la tabla de provedores y el campo puede ser null

                        int result = command.ExecuteNonQuery();
                        return result > 0; // Retorna true si se insertó correctamente
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }
        public bool ModificarProducto(Producto productoActualizado)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();
                    // El id no se actualiza y omitimos el id del provedor por ahora
                    string sql = @"UPDATE productos SET
                            nombre_producto = @nombre_producto,
                            marca_producto = @marca_producto,
                            presentacion_producto = @presentacion_producto,
                            unidad_medida_producto = @unidad_medida_producto,
                            precio_venta_producto = @precio_venta_producto,
                            precio_compra_producto = @precio_compra_producto,
                            categoria_producto = @categoria_producto
                        WHERE id_producto = @id_producto";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_producto", productoActualizado.id_producto);
                        command.Parameters.AddWithValue("@nombre_producto", productoActualizado.nombre_producto);
                        command.Parameters.AddWithValue("@marca_producto", productoActualizado.marca_producto);
                        command.Parameters.AddWithValue("@presentacion_producto", productoActualizado.presentacion_producto);
                        command.Parameters.AddWithValue("@unidad_medida_producto", productoActualizado.unidad_medida_producto);
                        command.Parameters.AddWithValue("@precio_venta_producto", productoActualizado.precio_venta_producto);
                        command.Parameters.AddWithValue("@precio_compra_producto", productoActualizado.precio_compra_producto);
                        //command.Parameters.AddWithValue("@estado_producto", productoActualizado.estado_producto);
                        command.Parameters.AddWithValue("@categoria_producto", productoActualizado.categoria_producto);
                        //command.Parameters.AddWithValue("@id_proveedor_producto", productoActualizado.id_proveedor_producto);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar producto: " + ex.Message);
                    return false;
                }
            }
        }
        public bool EliminarProducto(int idProducto)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = "DELETE FROM productos WHERE id_producto = @id_producto";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_producto", idProducto);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar producto: " + ex.Message);
                    return false;
                }
            }
        }// Tengo que cambiarlo, no se elimina se inabilitan

        /// <summary>
        /// Funciones para el Login
        /// </summary>
        public bool validar_inicio_sesion(string usuario, string contrasena)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT usuario, contrasena FROM usuarios";
                    using (var command = new MySqlCommand(sql, conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string usuarioObtenido = reader.GetString(0);
                            string contrasenaObtenida = reader.GetString(1);
                            if (usuario == usuarioObtenido && contrasena == contrasenaObtenida)
                                return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al validar el inicio de sesion con la base de datos: " + ex.Message);
                    return false;
                }
            }
        }
        public bool validar_usuarios_repetidos(string usuario)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT usuario FROM usuarios";
                    using (var command = new MySqlCommand(sql, conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string usuarioObtenido = reader.GetString(0);
                            if (usuario == usuarioObtenido)
                                return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al validar el usuario en la base de datos: " + ex.Message);
                    return false;
                }
            }
        }
        public bool ingresar_usuarios(Usuario usuarioAux)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();

                    string sql = @"INSERT INTO usuarios (id_empleado, usuario, contrasena, rol_usuario)
                                   VALUES (@id_empleado, @usuario, @contrasena, @rol_usuario)";

                    using (var command = new MySqlCommand(sql, conexion))
                    {
                        command.Parameters.AddWithValue("@id_empleado", usuarioAux.id_empleado);
                        command.Parameters.AddWithValue("@usuario", usuarioAux.usuario);
                        command.Parameters.AddWithValue("@contrasena", usuarioAux.contrasena);
                        command.Parameters.AddWithValue("@rol_usuario", usuarioAux.rol_usuario);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al ingresar el usuario a la base de datos: " + ex.Message);
                    return false;
                }
            }
        }
        public bool CambiarEstadoProducto(int idProducto, int nuevoEstado)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();
                    string sql = @"UPDATE productos SET estado_producto = @estado_producto WHERE id_producto = @id_producto";
                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@estado_producto", nuevoEstado);
                        command.Parameters.AddWithValue("@id_producto", idProducto);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cambiar el estado del producto: " + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// Operaciones de productos en la base de datos
        /// </summary>
        public List<long> ObtenerIDsProveedores() {
            List<long> ids = new List<long>();

            using (var Conexion = new MySqlConnection(conexionString)) {
                try {
                    Conexion.Open();
                    string sql = "SELECT id_proveedor FROM proveedores WHERE estado_proveedor = 1";

                    using (var comando = new MySqlCommand(sql, Conexion))
                    using (var reader = comando.ExecuteReader()) {
                        while (reader.Read()) {
                            long id = reader.GetInt64("id_proveedor");
                            ids.Add(id);
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Error al obtener IDs de proveedores: {ex.Message}");
                }
            }

            return ids;
        }
    }
}