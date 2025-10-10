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
        private void btn_Salir_Click(object sender, RoutedEventArgs e)
        {
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
            RegistroUsuarios registroUsuarios = new RegistroUsuarios();
            registroUsuarios.Show();
            this.Hide();
        }
    }
}
