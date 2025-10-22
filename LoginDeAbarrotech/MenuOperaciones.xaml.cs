using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LoginDeAbarrotech
{
    /// <summary>
    /// Lógica de interacción para MenuOperaciones.xaml
    /// </summary>
    public partial class MenuOperaciones : Window {
        public MenuOperaciones()
        {
            InitializeComponent();
        }
        private bool tieneAcceso(string Usuario)// Ocupo la tabla de inicios de sesion o al menos eso creo jaja
        {
            ConexionBD conexion = new ConexionBD();
            if (conexion.ObtenerRoles(LoginAbarrotech.UsuarioGlobal) == "Administrador"){// Pues reslto que no ocupaba la tabla de inicios de sesion jaja, pero ya funcina eso
                return true;
            }
            return false;
        }
        private void btn_Salir_Click(object sender, RoutedEventArgs e)
        {
            // Operaciones necesarias para registrar el inicio de sesion
            
            ConexionBD conexion = new ConexionBD();

            string UsuarioIniciado = LoginAbarrotech.UsuarioGlobal;

            int idAux = conexion.ObtenerIdUsuario(UsuarioIniciado);
            DateTime fechaActual = DateTime.Now;

            int idInicio = conexion.ObtenerIdSesionMasReciente(idAux);

            conexion.RegistrarCierreSesion(idInicio, idAux, fechaActual, 1);

            this.Close();
            LoginAbarrotech login = new LoginAbarrotech();
            login.Show();
        }
        private void btn_Productos_Click(object sender, RoutedEventArgs e)
        {
            // Abrir la ventana de productos
            RegistroProductos registroProductos = new RegistroProductos();
            registroProductos.Show();
            this.Hide();
        }
        private void btn_Proveedores_Click(object sender, RoutedEventArgs e)
        {
            RegistroProveedores registroProveedores = new RegistroProveedores();
            registroProveedores.Show();
            this.Hide();
        }
        private void btn_Empleados_Click(object sender, RoutedEventArgs e)
        {
            RegistroEmpleados registroEmpleadoss = new RegistroEmpleados();
            registroEmpleadoss.Show();
            this.Hide();
        }
        private void btn_Usuarios_Click(object sender, RoutedEventArgs e)
        {
            // Ya no abrira el registro de usuarios si no una listá para ver los usuarios registrados
            // Supongo que solo un administrador deberia poder verlos
        }
    }
}
