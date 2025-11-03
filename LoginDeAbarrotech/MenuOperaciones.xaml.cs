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
using System.Windows.Media.Animation;

namespace LoginDeAbarrotech
{
    /// <summary>
    /// Lógica de interacción para MenuOperaciones.xaml
    /// </summary>
    public partial class MenuOperaciones : Window {
        private bool isMenuCollapsed = false;
        private ConexionBD conexion;

        public MenuOperaciones()
        {
            InitializeComponent();
            conexion = new ConexionBD();
            InitializeMenu();
        }
        private void InitializeMenu()
        {
            // Configurar información del usuario
            txt_WelcomeUser.Text = $"Bienvenido, {LoginAbarrotech.UsuarioGlobal}";
            txt_CurrentUser.Text = LoginAbarrotech.UsuarioGlobal;
            
            // Obtener y mostrar el rol del usuario
            string userRole = conexion.ObtenerRolDeUsuario(LoginAbarrotech.UsuarioGlobal);
            txt_UserRole.Text = string.IsNullOrEmpty(userRole) ? "Sin especificar" : userRole;
            
            // Configurar fecha y hora actual
            txt_LastLogin.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            
            // Cargar estadísticas del dashboard
            LoadDashboardData();
        }
        private void LoadDashboardData()
        {
            try
            {
                // Cargar conteo de empleados
                var empleados = conexion.ObtenerEmpleados();
                txt_EmpleadosCount.Text = empleados.Count.ToString();
                
                // Cargar conteo de productos
                var productos = conexion.ObtenerProductos();
                txt_ProductosCount.Text = productos.Count.ToString();
                
                // Cargar conteo de proveedores
                var proveedores = conexion.ObtenerProveedores();
                txt_ProveedoresCount.Text = proveedores.Count.ToString();
                
                // Estado de la base de datos
                txt_DbStatus.Text = "Conectada";
                dbStatusIndicator.Fill = new SolidColorBrush(Color.FromRgb(76, 175, 80)); // Verde
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar estado desconectado
                txt_DbStatus.Text = "Error de conexión";
                dbStatusIndicator.Fill = new SolidColorBrush(Color.FromRgb(244, 67, 54)); // Rojo
                MessageBox.Show("Error al cargar datos del dashboard: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btn_ToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuCollapse();
        }
        private void ToggleMenuCollapse()
        {
            var animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            animation.EasingFunction = new QuadraticEase();

            if (isMenuCollapsed)
            {
                // Expandir menú
                animation.To = 250;
                isMenuCollapsed = false;
                
                // Mostrar textos
                foreach (Button btn in FindVisualChildren<Button>(SidebarBorder))
                {
                    if (btn.Name != "btn_ToggleMenu")
                    {
                        btn.HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                }
            }
            else
            {
                // Colapsar menú
                animation.To = 60;
                isMenuCollapsed = true;
                
                // Ocultar textos y centrar iconos
                foreach (Button btn in FindVisualChildren<Button>(SidebarBorder))
                {
                    if (btn.Name != "btn_ToggleMenu")
                    {
                        btn.HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                }
            }

            SidebarColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        // Método auxiliar para encontrar elementos visuales hijos
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        private void UpdatePageTitle(string title, string subtitle)
        {
            txt_PageTitle.Text = title;
            txt_PageSubtitle.Text = subtitle;
        }
        private void btn_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageTitle("Dashboard Principal", "Resumen general del sistema");
            DashboardContent.Visibility = Visibility.Visible;
            LoadDashboardData();
        }
        private bool tieneAcceso(string Usuario)
        {
            ConexionBD conexion = new ConexionBD();
            string aux = conexion.ObtenerRolDeUsuario(LoginAbarrotech.UsuarioGlobal);
            return(aux == "Administrador" || aux == "Gerente");
        }

        // Botones
        private void btn_Salir_Click(object sender, RoutedEventArgs e)
        {
            // Operaciones necesarias para registrar el cierre de sesión
            try
            {
                string UsuarioIniciado = LoginAbarrotech.UsuarioGlobal;
                int idAux = conexion.ObtenerIdUsuario(UsuarioIniciado);
                DateTime fechaActual = DateTime.Now;
                int idInicio = conexion.ObtenerIdSesionMasReciente(idAux);
                
                conexion.RegistrarCierreSesion(idInicio, idAux, fechaActual, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar cierre de sesión: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            this.Close();
            LoginAbarrotech login = new LoginAbarrotech();
            login.Show();
        }
        private void btn_Productos_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageTitle("Gestión de Productos", "Administrar inventario y catálogo");
            // Abrir la ventana de productos
            RegistroProductos registroProductos = new RegistroProductos();
            registroProductos.Show();
            this.Hide();
        }
        private void btn_Proveedores_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageTitle("Gestión de Proveedores", "Administrar información de proveedores");
            RegistroProveedores registroProveedores = new RegistroProveedores();
            registroProveedores.Show();
            this.Hide();
        }
        private void btn_Empleados_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageTitle("Gestión de Empleados", "Administrar personal de la empresa");
            RegistroEmpleados registroEmpleados = new RegistroEmpleados();
            registroEmpleados.Show();
            this.Hide();
        }
        private void btn_Usuarios_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageTitle("Gestión de Usuarios", "Administrar usuarios del sistema");
            // Verificar si el usuario tiene acceso de administrador
            if (tieneAcceso(LoginAbarrotech.UsuarioGlobal))
            {
                RegistroUsuarios registroUsuarios = new RegistroUsuarios();
                registroUsuarios.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("No tienes permisos para acceder a esta sección. Solo los administradores pueden gestionar usuarios.", 
                               "Acceso Denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btn_Compras_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageTitle("Gestión de Compras", "Administrar compras y pedidos");
            MessageBox.Show("Funcionalidad de Compras en desarrollo", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void btn_Ventas_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageTitle("Gestión de Ventas", "Administrar ventas y facturación");
            MessageBox.Show("Funcionalidad de Ventas en desarrollo", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
