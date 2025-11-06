using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
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

        public ConexionBD() { }

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
        public bool obtenerIdEmpleadoDeUsuarios(int idEmpleado)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT id_empleado FROM usuarios";
                    using (var command = new MySqlCommand(sql, conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Id_Empleado_Obtenido = reader.GetInt32(0);
                            if (idEmpleado == Id_Empleado_Obtenido)
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

                    string sql = @"SELECT * FROM productos";
                    using (var command = new MySqlCommand(sql, Conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto productoAux = new Producto(
                                reader.GetInt32(0),       // id_producto
                                reader.GetString(1),      // nombre_producto
                                reader.GetString(2),      // marca_producto
                                reader.GetInt32(3),      // presentacion_producto
                                reader.GetString(4),      // unidad_medida_producto
                                reader.GetFloat(5),       // precio_venta_producto
                                reader.GetFloat(6),       // precio_compra_producto
                                reader.GetString(7),       // estado_producto
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
                                  `estado_producto` ,`precio_venta_producto`, `precio_compra_producto`, `categoria_producto`, `id_proveedor_producto`) 
                                   VALUES(@nombre_producto, @marca_producto, @presentacion_producto, @unidad_medida_producto, @estado_producto,
                                   @precio_venta_producto, @precio_compra_producto, @categoria_producto, @id_proveedor_producto);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        string estado = "Activo";
                        // Agregamos parámetros para evitar una inyecion de SQL
                        //command.Parameters.AddWithValue("@Id_Producto", nuevoProducto.idProducto); // Es autoincrementable
                        command.Parameters.AddWithValue("@nombre_producto", nuevoProducto.nombre_producto);
                        command.Parameters.AddWithValue("@marca_producto", nuevoProducto.marca_producto);
                        command.Parameters.AddWithValue("@presentacion_producto", nuevoProducto.presentacion_producto);
                        command.Parameters.AddWithValue("@unidad_medida_producto", nuevoProducto.unidad_medida_producto);
                        command.Parameters.AddWithValue("@estado_producto", estado); // Se colocara por defecto "Activo"
                        command.Parameters.AddWithValue("@precio_venta_producto", nuevoProducto.precio_venta_producto);
                        command.Parameters.AddWithValue("@precio_compra_producto", nuevoProducto.precio_compra_producto);
                        command.Parameters.AddWithValue("@categoria_producto", nuevoProducto.categoria_producto);
                        command.Parameters.AddWithValue("@id_proveedor_producto", nuevoProducto.id_proveedor_producto);

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
                    // El id no se actualiza
                    string sql = @"UPDATE productos SET
                            nombre_producto = @nombre_producto,
                            marca_producto = @marca_producto,
                            presentacion_producto = @presentacion_producto,
                            unidad_medida_producto = @unidad_medida_producto,
                            precio_venta_producto = @precio_venta_producto,
                            precio_compra_producto = @precio_compra_producto,
                            categoria_producto = @categoria_producto,
                            id_proveedor_producto = @id_proveedor_producto
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
                        command.Parameters.AddWithValue("@id_proveedor_producto", productoActualizado.id_proveedor_producto);

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

        /// <summary>
        /// Es una funcion que sirve para la interfaz de produto
        /// </summary>
        public List<string> ObtenerNombresProveedores() {
            List<string> nombres = new List<string>();

            using (var Conexion = new MySqlConnection(conexionString)) {
                try {
                    Conexion.Open();
                    string sql = "SELECT nombre_proveedor FROM proveedores";

                    using (var comando = new MySqlCommand(sql, Conexion))
                    using (var reader = comando.ExecuteReader()) {
                        while (reader.Read()) {
                            string nom = reader.GetString("nombre_proveedor");
                            nombres.Add(nom);
                        }
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"Error al obtener nombres de proveedores: {ex.Message}");
                }
            }

            return nombres;
        }
        public int ObtenerIdProveedor(string proveedor)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();
                    string sql = "SELECT id_proveedor FROM proveedores WHERE nombre_proveedor = @proveedor LIMIT 1";

                    using (var comando = new MySqlCommand(sql, Conexion))
                    {
                        comando.Parameters.AddWithValue("@proveedor", proveedor);
                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32("id_proveedor");
                            }
                        }
                    }
                    return 0; // No encontrado
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener el id del proveedor: {ex.Message}");
                    return 0; // Indicar error
                }
            }
        }
        public string ObtenerNombreProveedor(int id)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();
                    string sql = "SELECT nombre_proveedor FROM proveedores WHERE id_proveedor = @id LIMIT 1";

                    using (var comando = new MySqlCommand(sql, Conexion))
                    {
                        comando.Parameters.AddWithValue("@id", id);
                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString("nombre_proveedor");
                            }
                        }
                    }
                    return ""; // No encontrado
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al obtener el id del proveedor: {ex.Message}");
                    return ""; // Indicar error
                }
            }
        }

        /// <summary>
        /// Operaciones de proveedores en la base de datos
        /// </summary>
        public List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> listaProveedores = new List<Proveedor>();

            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"SELECT * FROM proveedores";
                    using (var command = new MySqlCommand(sql, Conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Proveedor proveedorAux = new Proveedor(
                                reader.GetInt32(0),       // id_proveedor
                                reader.GetString(1),      // nombre_proveedor
                                reader.GetString(2),      // responsable_proveedor
                                reader.GetString(3),      // direccion_proveedor
                                reader.GetInt64(4),       // telefono_proveedor
                                reader.GetString(5),      // email_proveedor
                                reader.GetString(6)       // estado_proveedor
                            );

                            listaProveedores.Add(proveedorAux);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                }
            }
            return listaProveedores;
        }
        public bool AgregarProveedor(Proveedor nuevoProveedor)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO proveedores 
                            (nombre_proveedor, responsable_proveedor, direccion_proveedor, telefono_proveedor, email_proveedor, estado_proveedor)
                           VALUES
                            (@nombre_proveedor, @responsable_proveedor, @direccion_proveedor, @telefono_proveedor, @email_proveedor, @estado_proveedor);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@nombre_proveedor", nuevoProveedor.nombre_proveedor);
                        command.Parameters.AddWithValue("@responsable_proveedor", nuevoProveedor.responsable_proveedor);
                        command.Parameters.AddWithValue("@direccion_proveedor", nuevoProveedor.direccion_proveedor);
                        command.Parameters.AddWithValue("@telefono_proveedor", nuevoProveedor.telefono_proveedor);
                        command.Parameters.AddWithValue("@email_proveedor", nuevoProveedor.email_proveedor);
                        command.Parameters.AddWithValue("@estado_proveedor", nuevoProveedor.estado_proveedor);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar proveedor: " + ex.Message);
                    return false;
                }
            }
        }
        public bool ModificarProveedor(Proveedor proveedorActualizado)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"UPDATE proveedores SET
                            nombre_proveedor = @nombre_proveedor,
                            responsable_proveedor = @responsable_proveedor,
                            direccion_proveedor = @direccion_proveedor,
                            telefono_proveedor = @telefono_proveedor,
                            email_proveedor = @email_proveedor,
                            estado_proveedor = @estado_proveedor
                        WHERE id_proveedor = @id_proveedor";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_proveedor", proveedorActualizado.id_proveedor);
                        command.Parameters.AddWithValue("@nombre_proveedor", proveedorActualizado.nombre_proveedor);
                        command.Parameters.AddWithValue("@responsable_proveedor", proveedorActualizado.responsable_proveedor);
                        command.Parameters.AddWithValue("@direccion_proveedor", proveedorActualizado.direccion_proveedor);
                        command.Parameters.AddWithValue("@telefono_proveedor", proveedorActualizado.telefono_proveedor);
                        command.Parameters.AddWithValue("@email_proveedor", proveedorActualizado.email_proveedor);
                        command.Parameters.AddWithValue("@estado_proveedor", proveedorActualizado.estado_proveedor);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar proveedor: " + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// Operaciones de empleados en la base de datos
        /// </summary>
        public List<Empleado> ObtenerEmpleados()
        {
            List<Empleado> listaEmpleados = new List<Empleado>();

            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"SELECT * FROM empleados";
                    using (var command = new MySqlCommand(sql, Conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Empleado empleadoAux = new Empleado(
                                reader.GetInt64(0),       // id_empleado
                                reader.GetString(1),      // nombre_empleado
                                reader.GetString(2),      // direccion_empleado
                                reader.GetInt64(3),       // telefono_empleado
                                reader.GetString(4),      // correo_electronico_empleado
                                reader.GetString(5),      // rol_empleado
                                reader.GetFloat(6),       // salario_empleado
                                reader.GetDateTime(7),    // fecha_inicio_contrato
                                reader.GetTimeSpan(8),    // hora_entrada_empleado
                                reader.GetTimeSpan(9),    // hora_salida_empleado
                                reader.GetString(10),      // estado_empleado
                                reader.GetDateTime(11)    // fecha_fin_contrato
                            );

                            listaEmpleados.Add(empleadoAux);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                }
            }
            return listaEmpleados;
        }
        public bool AgregarEmpleado(Empleado nuevoEmpleado)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO empleados
                            (nombre_empleado, direccion_empleado, telefono_empleado, correo_electronico_empleado, rol_empleado, salario_empleado, fecha_inicio_contrato, hora_entrada_empleado, hora_salida_empleado, estado_empleado, fecha_fin_contrato)
                           VALUES
                            (@nombre_empleado, @direccion_empleado, @telefono_empleado, @correo_electronico_empleado, @rol_empleado, @salario_empleado, @fecha_inicio_contrato, @hora_entrada_empleado, @hora_salida_empleado, @estado_empleado, @fecha_fin_contrato);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@nombre_empleado", nuevoEmpleado.nombre_empleado);
                        command.Parameters.AddWithValue("@direccion_empleado", nuevoEmpleado.direccion_empleado);
                        command.Parameters.AddWithValue("@telefono_empleado", nuevoEmpleado.telefono_empleado);
                        command.Parameters.AddWithValue("@correo_electronico_empleado", nuevoEmpleado.correo_electronico_empleado);
                        command.Parameters.AddWithValue("@rol_empleado", nuevoEmpleado.rol_empleado);
                        command.Parameters.AddWithValue("@salario_empleado", nuevoEmpleado.salario_empleado);
                        command.Parameters.AddWithValue("@fecha_inicio_contrato", nuevoEmpleado.fecha_inicio_contrato);
                        command.Parameters.AddWithValue("@hora_entrada_empleado", nuevoEmpleado.hora_entrada_empleado);
                        command.Parameters.AddWithValue("@hora_salida_empleado", nuevoEmpleado.hora_salida_empleado);
                        command.Parameters.AddWithValue("@estado_empleado", nuevoEmpleado.estado_empleado);
                        command.Parameters.AddWithValue("@fecha_fin_contrato", nuevoEmpleado.fecha_fin_contrato);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar empleado: " + ex.Message);
                    return false;
                }
            }
        }
        public bool ModificarEmpleado(Empleado empleadoActualizado)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"UPDATE empleados SET
                            nombre_empleado = @nombre_empleado,
                            direccion_empleado = @direccion_empleado,
                            telefono_empleado = @telefono_empleado,
                            correo_electronico_empleado = @correo_electronico_empleado,
                            rol_empleado = @rol_empleado,
                            salario_empleado = @salario_empleado,
                            fecha_inicio_contrato = @fecha_inicio_contrato,
                            hora_entrada_empleado = @hora_entrada_empleado,
                            hora_salida_empleado = @hora_salida_empleado,
                            estado_empleado = @estado_empleado,
                            fecha_fin_contrato = @fecha_fin_contrato
                        WHERE id_empleado = @id_empleado";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_empleado", empleadoActualizado.id_empleado);
                        command.Parameters.AddWithValue("@nombre_empleado", empleadoActualizado.nombre_empleado);
                        command.Parameters.AddWithValue("@direccion_empleado", empleadoActualizado.direccion_empleado);
                        command.Parameters.AddWithValue("@telefono_empleado", empleadoActualizado.telefono_empleado);
                        command.Parameters.AddWithValue("@correo_electronico_empleado", empleadoActualizado.correo_electronico_empleado);
                        command.Parameters.AddWithValue("@rol_empleado", empleadoActualizado.rol_empleado);
                        command.Parameters.AddWithValue("@salario_empleado", empleadoActualizado.salario_empleado);
                        command.Parameters.AddWithValue("@fecha_inicio_contrato", empleadoActualizado.fecha_inicio_contrato);
                        command.Parameters.AddWithValue("@hora_entrada_empleado", empleadoActualizado.hora_entrada_empleado);
                        command.Parameters.AddWithValue("@hora_salida_empleado", empleadoActualizado.hora_salida_empleado);
                        command.Parameters.AddWithValue("@estado_empleado", empleadoActualizado.estado_empleado);
                        command.Parameters.AddWithValue("@fecha_fin_contrato", empleadoActualizado.fecha_fin_contrato);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar empleado: " + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// Operaciones de usuarios en la base de datos
        /// </summary>
        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();

            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"SELECT * FROM usuarios";
                    using (var command = new MySqlCommand(sql, Conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario usuarioAux = new Usuario(
                                reader.GetInt32(0),       // id_empleado
                                reader.GetInt32(1),       // nombre_empleado
                                reader.GetString(2),      // direccion_empleado
                                reader.GetString(3),      // correo_electronico_empleado
                                reader.GetString(4)       // rol_empleado
                            );

                            listaUsuarios.Add(usuarioAux);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                }
            }
            return listaUsuarios;
        }
        public bool AgregarUsuarios(Usuario nuevoUsuario)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO usuarios (id_usuario, id_empleado, usuario, contrasena, rol_usuario)
                                   VALUES (@id_empleado, @usuario, @contrasena, @rol_usuario);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_usuario", nuevoUsuario.id_usuario);
                        command.Parameters.AddWithValue("@id_empleado", nuevoUsuario.id_empleado);
                        command.Parameters.AddWithValue("@usuario", nuevoUsuario.usuario);
                        command.Parameters.AddWithValue("@contrasena", nuevoUsuario.contrasena);
                        command.Parameters.AddWithValue("@rol_usuario", nuevoUsuario.rol_usuario);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar usuario: " + ex.Message);
                    return false;
                }
            }
        }
        public bool validarIdEmpleado(int idEmpleado)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT id_empleado FROM empleados";
                    using (var command = new MySqlCommand(sql, conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int IdObtenido = reader.GetInt32(0);
                            if (IdObtenido == idEmpleado)
                                return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al validar el id del empleado: " + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// Limitacion de acceso a los usuarios
        /// </summary>
        public string ObtenerRolDeUsuario(string Usuario)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT rol_usuario FROM usuarios WHERE usuario = @Usuario";

                    using (var command = new MySqlCommand(sql, conexion))
                    {
                        command.Parameters.AddWithValue("@Usuario", Usuario);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string rol_usuario_obtenido = reader.GetString(0);
                                return rol_usuario_obtenido;
                            }
                        }
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener el rol del usuario: " + ex.Message);
                    return "";
                }
            }
        }
        public string ObtenerRolDeEmpleado(int IdEmpleado)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT rol_empleado FROM empleados WHERE id_empleado = @IdEmpleado";

                    using (var command = new MySqlCommand(sql, conexion))
                    {
                        command.Parameters.AddWithValue("@IdEmpleado", IdEmpleado);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string rol_empleado_obtenido = reader.GetString(0);
                                return rol_empleado_obtenido;
                            }
                        }
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener el rol del empleado: " + ex.Message);
                    return "";
                }
            }
        }

        /// <summary>
        /// Registro de inicios y cierres de sesion
        /// </summary>
        public bool RegistrarInicioSesion(int idUsuario, DateTime fechaHora, int numeroCaja)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string sql = @"INSERT INTO inicios_sesion (id_usuario, fecha_hora_inicio_sesion, numero_caja)
                           VALUES (@id_usuario, @fecha_hora_inicio_sesion, @numero_caja);";
                    using (var command = new MySqlCommand(sql, conexion))
                    {
                        command.Parameters.AddWithValue("@id_usuario", idUsuario);
                        command.Parameters.AddWithValue("@fecha_hora_inicio_sesion", fechaHora);
                        command.Parameters.AddWithValue("@numero_caja", numeroCaja);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error al registrar inicio de sesión: " + ex.Message);
                    // Luego lo arreglo jaja, es por el indice en la tabla de inicios de sesion
                    return false;
                }
            }
        }
        public bool RegistrarCierreSesion(int idInicioSesion, int idUsuario, DateTime fechaHora, int numeroCaja)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string sql = @"INSERT INTO cierres_sesion (id_inicio_sesion ,id_usuario, fecha_hora_cierre_sesion, caja)
                           VALUES (@id_inicio_sesion, @id_usuario, @fecha_hora_cierre_sesion, @caja);";
                    using (var command = new MySqlCommand(sql, conexion))
                    {
                        command.Parameters.AddWithValue("@id_inicio_sesion", idInicioSesion);
                        command.Parameters.AddWithValue("@id_usuario", idUsuario);
                        command.Parameters.AddWithValue("@fecha_hora_cierre_sesion", fechaHora);
                        command.Parameters.AddWithValue("@caja", numeroCaja);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar cierre de sesión: " + ex.Message);
                    return false;
                }
            }
        }
        public int ObtenerIdUsuario(string Usuario)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string sql = "SELECT id_usuario FROM usuarios WHERE usuario = @Usuario";
                    using (var command = new MySqlCommand(sql, conexion))
                    {
                        command.Parameters.AddWithValue("@usuario", Usuario);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                return reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener id_usuario: " + ex.Message);
                }
            }
            return 0;
        }
        public int ObtenerIdSesionMasReciente(int idUsuario)
        {
            using (var conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    conexion.Open();
                    string sql = @"SELECT id_inicio_sesion 
                           FROM inicios_sesion 
                           WHERE id_usuario = @id_usuario 
                           ORDER BY fecha_hora_inicio_sesion DESC 
                           LIMIT 1";
                    // El DESC es para que ordene las fechas en orden decendente
                    // y el LIMIT 1 es para que solo muestre un registo en el resultado de la consulta
                    using (var command = new MySqlCommand(sql, conexion))
                    {
                        command.Parameters.AddWithValue("@id_usuario", idUsuario);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                return reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener el id de inicio de sesión más reciente: " + ex.Message);
                }
            }
            return 0;
        }

        /// <summary>
        /// Operaciones de ventas
        /// </summary>
        public bool RealizarVenta(long idUsuario, DateTime fechaVenta, float totalVenta, string formaPagoVenta, int caja)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO `ventas`
                           (`id_usuario`, `fecha_venta`, `total_venta`, `forma_pago_venta`, `caja`)
                           VALUES
                           (@id_usuario, @fecha_venta, @total_venta, @forma_pago_venta, @caja);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_usuario", idUsuario);
                        command.Parameters.AddWithValue("@fecha_venta", fechaVenta);
                        command.Parameters.AddWithValue("@total_venta", totalVenta);
                        command.Parameters.AddWithValue("@forma_pago_venta", formaPagoVenta);
                        command.Parameters.AddWithValue("@caja", caja);

                        int result = command.ExecuteNonQuery();
                        return result > 0; // True si se registró la venta
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en la venta: " + ex.Message);
                    return false;
                }
            }
        }
        public bool AgregarDetalleVenta(long idVenta, long idProducto, float cantidadVenta, float precioVenta)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO `detalle_ventas`
                           (`id_venta`, `id_producto`, `cantidad_venta`, `precio_venta`)
                           VALUES
                           (@id_venta, @id_producto, @cantidad_venta, @precio_venta);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_venta", idVenta);
                        command.Parameters.AddWithValue("@id_producto", idProducto);
                        command.Parameters.AddWithValue("@cantidad_venta", cantidadVenta);
                        command.Parameters.AddWithValue("@precio_venta", precioVenta);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar el detalle de venta: " + ex.Message);
                    return false;
                }
            }
        }
        public List<Producto> ObtenerProductosDisponibles()
        {
            List<Producto> listaProductos = new List<Producto>();

            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"SELECT * FROM productos WHERE estado_producto = 'Activo'";
                    using (var command = new MySqlCommand(sql, Conexion))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto productoAux = new Producto(
                                reader.GetInt32(0),       // id_producto
                                reader.GetString(1),      // nombre_producto
                                reader.GetString(2),      // marca_producto
                                reader.GetInt32(3),      // presentacion_producto
                                reader.GetString(4),      // unidad_medida_producto
                                reader.GetFloat(5),       // precio_venta_producto
                                reader.GetFloat(6),       // precio_compra_producto
                                reader.GetString(7),       // estado_producto
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
        public List<Producto> ObtenerProductosPorCategoria(string Categoria, string busqueda = null)
        {
            List<Producto> listaProductos = new List<Producto>();
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    //  Filtra por categoría exacta (@Categoria)
                    //  Solo productos Activo
                    //  Si @Patron es NULL, NO aplica filtro por nombre (se omite).
                    //  Si NO es NULL, aplica un LIKE con LOWER(nombre_producto) para coincidir sin distinguir mayúsculas/minúsculas.
                    string sql = @"
                            SELECT * FROM productos WHERE categoria_producto = @Categoria 
                            AND estado_producto = 'Activo'
                            AND (@Patron IS NULL OR LOWER(nombre_producto) LIKE @Patron)";
                            // LiKE es para busquedas parciales, puedes colocar el unicio, final o un conjunto de letras para que haga esta busqueda 

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@Categoria", Categoria);

                        // Construye el patrón de búsqueda si hay texto:
                        // Trim quita espacios al inicio/fin
                        // ToLower para comparar en minúsculas (acompañado por LOWER en SQL)
                        // %...% para coincidencia parcial en cualquier posición
                        string patron = string.IsNullOrWhiteSpace(busqueda)
                            ? null
                            : $"%{busqueda.Trim().ToLower()}%";

                        // Si no hay texto, se manda NULL => el predicado (@Patron IS NULL OR ...) se cumple y se omite el filtro
                        if (patron is null)
                            command.Parameters.AddWithValue("@Patron", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Patron", patron);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Producto productoAux = new Producto(
                                    reader.GetInt32(0),       // id_producto
                                    reader.GetString(1),      // nombre_producto
                                    reader.GetString(2),      // marca_producto
                                    reader.GetInt32(3),       // presentacion_producto
                                    reader.GetString(4),      // unidad_medida_producto
                                    reader.GetFloat(5),       // precio_venta_producto
                                    reader.GetFloat(6),       // precio_compra_producto
                                    reader.GetString(7),      // estado_producto
                                    reader.GetString(8),      // categoria_producto
                                    reader.GetInt32(9)        // id_proveedor_producto
                                );

                                listaProductos.Add(productoAux);
                            }
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
        public List<Producto> ObtenerProductosPorCoincidencia(string busqueda)
        {
            List<Producto> listaProductos = new List<Producto>();
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    //  Filtra por categoría exacta (@Categoria)
                    //  Solo productos Activo
                    //  Si @Patron es NULL, NO aplica filtro por nombre (se omite).
                    //  Si NO es NULL, aplica un LIKE con LOWER(nombre_producto) para coincidir sin distinguir mayúsculas/minúsculas.
                    string sql = @"
                            SELECT * FROM productos WHERE estado_producto = 'Activo'
                            AND (@Patron IS NULL OR LOWER(nombre_producto) LIKE @Patron)";
                    // LiKE es para busquedas parciales, puedes colocar el unicio, final o un conjunto de letras para que haga esta busqueda 

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        // Construye el patrón de búsqueda si hay texto:
                        // Trim quita espacios al inicio/fin
                        // ToLower para comparar en minúsculas (acompañado por LOWER en SQL)
                        // %...% para coincidencia parcial en cualquier posición
                        string patron = string.IsNullOrWhiteSpace(busqueda)
                            ? null
                            : $"%{busqueda.Trim().ToLower()}%";

                        // Si no hay texto, se manda NULL => el predicado (@Patron IS NULL OR ...) se cumple y se omite el filtro
                        if (patron is null)
                            command.Parameters.AddWithValue("@Patron", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Patron", patron);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Producto productoAux = new Producto(
                                    reader.GetInt32(0),       // id_producto
                                    reader.GetString(1),      // nombre_producto
                                    reader.GetString(2),      // marca_producto
                                    reader.GetInt32(3),       // presentacion_producto
                                    reader.GetString(4),      // unidad_medida_producto
                                    reader.GetFloat(5),       // precio_venta_producto
                                    reader.GetFloat(6),       // precio_compra_producto
                                    reader.GetString(7),      // estado_producto
                                    reader.GetString(8),      // categoria_producto
                                    reader.GetInt32(9)        // id_proveedor_producto
                                );

                                listaProductos.Add(productoAux);
                            }
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

        /// <summary>
        /// Operaciones de compras
        /// </summary>
        public bool RealizarCompra(long idUsuario, DateTime fechaCompra, float totalCompra, string formaPagoCompra, int caja)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO `compras`
                           (`id_usuario`, `fecha_compra`, `total_compra`, `forma_pago_compra`, `caja`)
                           VALUES
                           (@id_usuario, @fecha_compra, @total_compra, @forma_pago_compras, @caja);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_usuario", idUsuario);
                        command.Parameters.AddWithValue("@fecha_compra", fechaCompra);
                        command.Parameters.AddWithValue("@total_compra", totalCompra);
                        command.Parameters.AddWithValue("@forma_pago_compra", formaPagoCompra);
                        command.Parameters.AddWithValue("@caja", caja);

                        int result = command.ExecuteNonQuery();
                        return result > 0; // True si se registró la venta
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en la venta: " + ex.Message);
                    return false;
                }
            }
        }
        public bool AgregarDetalleCompra(long idCompra, long idProducto, float cantidadCompra, float precioVenta)// En la bd dice precioVenta
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO `detalle_compras`
                           (`id_compra`, `id_producto`, `cantidad_compra`, `precio_venta`)
                           VALUES
                           (@id_compra, @id_producto, @cantidad_compra, @precio_venta);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_compra", idCompra);
                        command.Parameters.AddWithValue("@id_producto", idProducto);
                        command.Parameters.AddWithValue("@cantidad_compra", cantidadCompra);
                        command.Parameters.AddWithValue("@precio_venta", precioVenta);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar el detalle de compra: " + ex.Message);
                    return false;
                }
            }
        }
        /// <summary>
        /// Operaciones de transacciones e inventario
        /// </summary>
        public bool RealizarTransaccion(long idInventario, long idInicioSesion, string tipoTransacción, int cantidadModificada, DateTime fechaRegistro, long idVenta, long idCompra)// Revisar nulos
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();

                    string sql = @"INSERT INTO `transacciones`
                           (`id_inventario`, `id_inicio_sesion`, `tipo_movimiento_transaccion`, `cantidad_modificada_transaccion`, `fecha_registro_salida_transaccion`, `id_venta`, `id_compra`)
                           VALUES
                           (@id_inventario, @id_inicio_sesion, @tipo_movimiento_transaccion, @cantidad_modificada_transaccion, @fecha_registro_salida_transaccion, @id_venta, @id_compra);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_inventario", idInventario);
                        command.Parameters.AddWithValue("@id_inicio_sesion", idInicioSesion);
                        command.Parameters.AddWithValue("@tipo_movimiento_transaccion", tipoTransacción);
                        command.Parameters.AddWithValue("@cantidad_modificada_transaccion", cantidadModificada);
                        command.Parameters.AddWithValue("@fecha_registro_salida_transaccion", fechaRegistro);
                        command.Parameters.AddWithValue("@id_venta", idVenta);// Debemos considerar dejar uno nulo cuando hagamos las transacciones
                        command.Parameters.AddWithValue("@id_compra", idCompra);// ...

                        int result = command.ExecuteNonQuery();
                        return result > 0; // True si se registró la transacción
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar la transacción: " + ex.Message);
                    return false;
                }
            }
        }
        public bool AgregarInventario(long idProducto, string codigoBarras, int cantidad, string ubicacion, DateTime fechaElaboracion, DateTime fechaCaducidad)
        {
            using (var Conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    Conexion.Open();
                    string sql = @"INSERT INTO `inventario`
                           (`id_producto`, `codigo_barras_inventario`, `cantidad_inventario`, `ubicacion_inventario`, `fecha_elaboracion`, `fecha_caducidad`)
                           VALUES
                           (@id_producto, @codigo_barras_inventario, @cantidad_inventario, @ubicacion_inventario, @fecha_elaboracion, @fecha_caducidad);";

                    using (var command = new MySqlCommand(sql, Conexion))
                    {
                        command.Parameters.AddWithValue("@id_producto", idProducto);
                        command.Parameters.AddWithValue("@codigo_barras_inventario", codigoBarras);
                        command.Parameters.AddWithValue("@cantidad_inventario", cantidad);
                        command.Parameters.AddWithValue("@ubicacion_inventario", ubicacion);
                        command.Parameters.AddWithValue("@fecha_elaboracion", fechaElaboracion);
                        command.Parameters.AddWithValue("@fecha_caducidad", fechaCaducidad);

                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar inventario: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
