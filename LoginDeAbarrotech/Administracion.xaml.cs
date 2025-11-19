using System;
using System.Windows;

namespace LoginDeAbarrotech
{
    public partial class Administracion : Window
    {
        public Administracion()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            ConexionBD conexion = new ConexionBD();
            
            // Cargar ventas
            dg_Ventas.ItemsSource = conexion.ObtenerVentas();

            // Cargar compras
            dg_Compras.ItemsSource = conexion.ObtenerCompras();

            // Cargar transacciones
            dg_Transacciones.ItemsSource = conexion.ObtenerTransacciones();

            // Cargar inventario
            dg_Inventario.ItemsSource = conexion.ObtenerInventario();
        }

        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Close();
        }
    }
}
