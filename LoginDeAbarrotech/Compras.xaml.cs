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
    /// Lógica de interacción para Compras.xaml
    /// </summary>
    public partial class Compras : Window
    {
        public Compras()
        {
            InitializeComponent();
            CargarProductos();
            CargarProveedores();
        }
        private void btn_filtrar_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btl_LimpiarFiltro_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CargarProductos()
        {
            ConexionBD conexion = new ConexionBD();
            dg_ProductosDisponibles.ItemsSource = conexion.ObtenerProductos();
        }
        private void CargarProveedores()
        {
            ConexionBD conexion = new ConexionBD();
            cb_proveedores.ItemsSource = conexion.ObtenerProveedoresActivos();
        }
        /// <summary>
        /// La clase "ProductoSeleccionado" se usa para agregarle un campo de cantidad a los productos, se hace herencia
        /// </summary>
        internal class ProductoSeleccionado : Producto
        {
            public int cantidad { get; set; } = 0;
            public float Subtotal => cantidad * precio_venta_producto; // cálculo simple
        }
        float total = 0.0f;// Total de la compra

        internal static List<ProductoSeleccionado> productosSeleccionados = new List<ProductoSeleccionado>();
        private void btn_RealizarCompra_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btn_cancelar_Click_1(object sender, RoutedEventArgs e)
        {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Hide();
        }
        private void dg_ProductosDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
