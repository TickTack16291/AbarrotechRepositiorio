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
        public void MostrarMensaje()
        {
            Lbl_mensaje.Visibility = Visibility.Visible;
            var animacion = new System.Windows.Media.Animation.ThicknessAnimation();
            animacion.Duration = TimeSpan.FromMilliseconds(100);
            animacion.From = new Thickness(0);
            animacion.To = new Thickness(5);
            animacion.AutoReverse = true;
            animacion.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(2);
            Lbl_mensaje.BeginAnimation(MarginProperty, animacion);
        }
        private void VaciarTablaSeleccionados()
        {
            dg_ProductosSeleecionados.Items.Clear();
            productosSeleccionados.Clear();
            total = 0f;
            Txt_TotalCompra.Text = "0";
        }
        public void CargarProductosPorProveedor(int aux1, string aux2)
        {
            ConexionBD conexion = new ConexionBD();
            List<Producto> producto;
            if (aux2 == string.Empty)
                producto = conexion.ObtenerProductosPorProveedor(aux1, null);
            else
                producto = conexion.ObtenerProductosPorProveedor(aux1, aux2);

            dg_ProductosDisponibles.ItemsSource = producto;
        }
        public void CargarProductosPorCoincidencia(string aux1)
        {
            ConexionBD conexion = new ConexionBD();
            var producto = conexion.ObtenerProductosPorCoincidenciaTodos(aux1);
            dg_ProductosDisponibles.ItemsSource = producto;
        }
        private void btn_filtrar_Click(object sender, RoutedEventArgs e)
        {
            if(cb_proveedores.Text == "Todos")
            {
                CargarProductosPorCoincidencia(txt_busqueda.Text);
            } else
            {
                ConexionBD conexion = new ConexionBD();
                CargarProductosPorProveedor(conexion.ObtenerIdProveedor(cb_proveedores.Text), txt_busqueda.Text);
            }
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
            // Se hace esto por que si agregamos "Todos" directamente al combobox no se puede usar el ItemSource
            ConexionBD conexion = new ConexionBD();
            var proveedores = conexion.ObtenerProveedoresActivos();
            
            var listaCompleta = new List<string> { "Todos" };
            listaCompleta.AddRange(proveedores);
            
            cb_proveedores.ItemsSource = listaCompleta;
            cb_proveedores.SelectedIndex = 0;
        }
        private void btn_cancelar_Click_1(object sender, RoutedEventArgs e)
        {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Hide();
        }
        private void cb_proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return; // Evita ejecutar mientras la ventana se inicializa
            txt_busqueda.Clear();

            ConexionBD conexion = new ConexionBD();
            string proveedor = (e.AddedItems.Count > 0 ? (e.AddedItems[0] as ComboBoxItem)?.Content?.ToString() : null)
                       ?? cb_proveedores.SelectedValue?.ToString()
                       ?? cb_proveedores.Text;

            CargarProductosPorProveedor(conexion.ObtenerIdProveedor(proveedor), "");
        }

        /// <summary>
        /// La clase "ProductoSeleccionado" se usa para agregarle un campo de cantidad a los productos, se hace herencia
        /// </summary>
 
        internal class ProductoSeleccionadoCompra : Producto
        {
            public int cantidad { get; set; } = 0;
            public float Subtotal => cantidad * precio_compra_producto; // Usar precio_compra_producto en compras
        }
        float total = 0.0f;// Total de la compra
        internal static List<ProductoSeleccionadoCompra> productosSeleccionados = new List<ProductoSeleccionadoCompra>();// Esta aqui para que no se reinicie cada que se llame el evento
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
                var prAux = new ProductoSeleccionadoCompra
                {
                    id_producto = prseleccionado.id_producto,
                    nombre_producto = prseleccionado.nombre_producto,
                    id_proveedor_producto = prseleccionado.id_proveedor_producto,
                    precio_compra_producto = prseleccionado.precio_compra_producto, // CORREGIDO: usar precio_compra_producto
                    cantidad = 1
                };

                productosSeleccionados.Add(prAux);
                dg_ProductosSeleecionados.Items.Add(prAux);

                total += prAux.precio_compra_producto; // CORREGIDO: usar precio_compra_producto
            }
            else
            {
                existente.cantidad++;
                total += existente.precio_compra_producto; // CORREGIDO: usar precio_compra_producto
            }

            Txt_TotalCompra.Text = total.ToString();
            dg_ProductosSeleecionados.Items.Refresh();// Es para forzar que se actualize la tabla
            dg_ProductosDisponibles.SelectedItem = null;
        }
        private void btn_RealizarCompra_Click(object sender, RoutedEventArgs e)
        {
            ConexionBD conexion = new ConexionBD();

            if (total == 0)
            {
                MostrarMensaje();
                return;
            }

            ResumenCompra resumenCompra = new ResumenCompra();
            resumenCompra.ShowDialog();

            if (!resumenCompra.cancelada)
                VaciarTablaSeleccionados();// No se vacia si se cancelo, por que podria ser para seleccionar o quitar productos
        }
        private void dg_ProductosSeleecionados_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var prod = dg_ProductosSeleecionados.SelectedItem as ProductoSeleccionadoCompra;
            if (prod == null) return;

            // Disminuir en 1 la cantidad y ajustar total
            total -= prod.precio_venta_producto;
            if (total < 0) total = 0f;

            prod.cantidad--;

            if (prod.cantidad <= 0)
            {
                productosSeleccionados.Remove(prod);
                dg_ProductosSeleecionados.Items.Remove(prod);
            }

            Txt_TotalCompra.Text = total.ToString();
            dg_ProductosSeleecionados.Items.Refresh();

            // Limpia selección para permitir nuevo clic
            dg_ProductosSeleecionados.SelectedItem = null;
        }
    }
}
