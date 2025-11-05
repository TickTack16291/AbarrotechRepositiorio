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
                CargarProductosDisponibles();
            } else
            {
                CargarProductosPorCategoria(cb_categorias.Text, txt_busqueda.Text);
            }
        }
        private void cb_categorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Da un error que no entiendo, lastima es un buen detalle jaja
        }
        
        List<Producto> productosSeleccionados = new List<Producto>();
        private void dg_ProductosDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Producto prseleccionado = dg_ProductosDisponibles.SelectedItem as Producto;

            if (prseleccionado != null)
            {
                productosSeleccionados.Add(prseleccionado);
                dg_ProductosSeleecionados.Items.Add(prseleccionado);
            }
        }
    }
}
