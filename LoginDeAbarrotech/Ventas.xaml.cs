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
    /// Lógica de interacción para Ventas.xaml
    /// </summary>
    public partial class Ventas : Window
    {
        public Ventas()
        {
            InitializeComponent();
            CargarProductosDisponibles();
        }
        public void CargarProductosDisponibles()
        {
            ConexionBD conexion = new ConexionBD();
            var producto = conexion.ObtenerProductosDisponibles();
            dg_ProductosDisponibles.ItemsSource = producto;
        }
        public void CargarProductosPorCategoria(string aux1, string aux2)
        {
            ConexionBD conexion = new ConexionBD();
            var producto = conexion.ObtenerProductosPorCategoria(aux1, aux2);
            dg_ProductosDisponibles.ItemsSource = producto;
        }
        public void CargarProductosPorCoincidencia(string aux1)
        {
            ConexionBD conexion = new ConexionBD();
            var producto = conexion.ObtenerProductosPorCoincidencia(aux1);
            dg_ProductosDisponibles.ItemsSource = producto;
        }
        private void btn_cancelar_Click_1(object sender, RoutedEventArgs e)
        {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Hide();
        }
        private void btl_LimpiarFiltro_Click(object sender, RoutedEventArgs e)
        {
            txt_busqueda.Text = string.Empty;
            cb_categorias.Text = "Todas";
            CargarProductosDisponibles();
        }
        private void btn_filtrar_Click(object sender, RoutedEventArgs e)
        {
            if(cb_categorias.Text == "Todas")
            {
                CargarProductosPorCoincidencia(txt_busqueda.Text);
            } else
            {
                CargarProductosPorCategoria(cb_categorias.Text, txt_busqueda.Text);
            }
        }
        private void cb_categorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Da un error que no entiendo, lastima es un buen detalle jaja
        }
        /// <summary>
        /// La clase "ProductoSeleccionado se usa para agregarle un campo de cantidad a los productos, se hace herencia"
        /// </summary>
        internal class ProductoSeleccionado : Producto
        {
            public int cantidad { get; set; } = 0;
        }
        float total = 0.0f;// Total de la compra
        List<ProductoSeleccionado> productosSeleccionados = new List<ProductoSeleccionado>();// Esta aqui para que no se reinicie cada que se llame el evento
        private void dg_ProductosDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var prseleccionado = dg_ProductosDisponibles.SelectedItem as Producto;
            if (prseleccionado == null) return;

            // Busca si ya se habia seleccionado ese producto
            // Se recorre la lista hasta encontrar la primer coincidencia o puede no encontrarla en dico caso devuelve un valor nulo
            var existente = productosSeleccionados.FirstOrDefault(p => p.id_producto == prseleccionado.id_producto);// Expresion lamda que toma p que es un
                                                                                                                    // producto seleccionado y lo usa para
                                                                                                                    // comparar y devolver un valor boleano

            if (existente == null)
            {
                var prAux = new ProductoSeleccionado
                {
                    id_producto = prseleccionado.id_producto,
                    nombre_producto = prseleccionado.nombre_producto,
                    categoria_producto = prseleccionado.categoria_producto,
                    precio_venta_producto = prseleccionado.precio_venta_producto,
                    cantidad = 1
                };

                productosSeleccionados.Add(prAux);
                dg_ProductosSeleecionados.Items.Add(prAux);

                total += prAux.precio_venta_producto;
            }
            else
            {
                existente.cantidad++;
                total += existente.precio_venta_producto;
            }

            Txt_TotalVenta.Text = total.ToString();
            dg_ProductosSeleecionados.Items.Refresh();// Es para forzar que se actualize la tabla
            dg_ProductosDisponibles.SelectedItem = null;
        }
        private void btn_RealizarVenta_Click(object sender, RoutedEventArgs e)
        {
            ConexionBD conexion = new ConexionBD();
            long idAux = conexion.ObtenerIdUsuario(LoginAbarrotech.UsuarioGlobal);
            conexion.RealizarVenta(idAux, DateTime.Now, total, "Efectivo", 1);
            // Habria que hacer una ventana en donde selecciones el metodo de pago ademas de que te deberia dar un resumen de la venta
            // Total - Cant. de productos - Usuario que realiza la venta - hora y fecha - numero de caja
        }
    }
}
