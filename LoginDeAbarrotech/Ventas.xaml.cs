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
        internal static List<ProductoSeleccionadoVenta> productosSeleccionados = new List<ProductoSeleccionadoVenta>();// Esta aqui para que no se reinicie cada que se llame el evento
        public Ventas()
        {
            InitializeComponent();
            CargarProductosDisponibles();
            productosSeleccionados.Clear();
        }
        public void MostrarMensaje(string mensaje = null)
        {
            if(mensaje != null)
                Lbl_mensaje.Content = mensaje;

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
            Txt_TotalVenta.Text = "0";
        }

        /// <summary>
        /// Clase para mostrar productos con su stock en la interfaz
        /// </summary>
        internal class ProductoConStock : Producto
        {
            public int Stock { get; set; }
        }

        /// <summary>
        /// Método auxiliar para convertir productos a productos con stock
        /// </summary>
        private List<ProductoConStock> AgregarStockAProductos(List<Producto> productos)
        {
            ConexionBD conexion = new ConexionBD();
            var stock = conexion.ObtenerStockInventario();

            return productos.Select(p => new ProductoConStock
            {
                id_producto = p.id_producto,
                nombre_producto = p.nombre_producto,
                marca_producto = p.marca_producto,
                presentacion_producto = p.presentacion_producto,
                unidad_medida_producto = p.unidad_medida_producto,
                precio_venta_producto = p.precio_venta_producto,
                precio_compra_producto = p.precio_compra_producto,
                estado_producto = p.estado_producto,
                categoria_producto = p.categoria_producto,
                id_proveedor_producto = p.id_proveedor_producto,
                Stock = stock.ContainsKey(p.id_producto) ? stock[p.id_producto] : 0
            }).ToList();
        }
        public void CargarProductosDisponibles()
        {
            ConexionBD conexion = new ConexionBD();
            var productos = conexion.ObtenerProductosDisponibles();
            dg_ProductosDisponibles.ItemsSource = AgregarStockAProductos(productos);
        }
        public void CargarProductosPorCategoria(string aux1, string aux2)
        {
            ConexionBD conexion = new ConexionBD();
            List<Producto> productos;
            if (aux2 == string.Empty)
                productos = conexion.ObtenerProductosPorCategoria(aux1, null);
            else
                productos = conexion.ObtenerProductosPorCategoria(aux1, aux2);

            dg_ProductosDisponibles.ItemsSource = AgregarStockAProductos(productos);
        }
        public void CargarProductosPorCoincidencia(string aux1)
        {
            ConexionBD conexion = new ConexionBD();
            var productos = conexion.ObtenerProductosPorCoincidenciaActivos(aux1);
            dg_ProductosDisponibles.ItemsSource = AgregarStockAProductos(productos);
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
            if (!IsLoaded) return; // Evita ejecutar mientras la ventana se inicializa
            txt_busqueda.Clear();

            string categoria = (e.AddedItems.Count > 0 ? (e.AddedItems[0] as ComboBoxItem)?.Content?.ToString() : null)
                       ?? cb_categorias.SelectedValue?.ToString()
                       ?? cb_categorias.Text;

            CargarProductosPorCategoria(categoria, "");
        }
        /// <summary>
        /// La clase "ProductoSeleccionado" se usa para agregarle un campo de cantidad a los productos, se hace herencia
        /// </summary>
        internal class ProductoSeleccionadoVenta : Producto
        {
            public int cantidad { get; set; } = 0;
            public float Subtotal => cantidad * precio_venta_producto; // cálculo simple
        }
        float total = 0.0f;// Total de la compra

        private void dg_ProductosDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var prseleccionado = dg_ProductosDisponibles.SelectedItem as Producto;
            var prStock = dg_ProductosDisponibles.SelectedItem as ProductoConStock;

            if (prseleccionado == null) return;

            // Busca si ya se habia seleccionado ese producto
            // Se recorre la lista hasta encontrar la primer coincidencia o puede no encontrarla en dico caso devuelve un valor nulo
            var existente = productosSeleccionados.FirstOrDefault(p => p.id_producto == prseleccionado.id_producto);// Expresion lamda que toma p que es un
                                                                                                                    // producto seleccionado y lo usa para
                                                                                                                    // comparar y devolver un valor boleano

            if (existente == null)// No se habia seleccionado ese producto
            {
                var prAux = new ProductoSeleccionadoVenta
                {
                    id_producto = prseleccionado.id_producto,
                    nombre_producto = prseleccionado.nombre_producto,
                    categoria_producto = prseleccionado.categoria_producto,
                    precio_venta_producto = prseleccionado.precio_venta_producto,
                    id_proveedor_producto = prseleccionado.id_proveedor_producto,
                    presentacion_producto = prseleccionado.presentacion_producto,
                    unidad_medida_producto = prseleccionado.unidad_medida_producto,
                    cantidad = 1
                };

                productosSeleccionados.Add(prAux);
                dg_ProductosSeleecionados.Items.Add(prAux);

                total += prAux.precio_venta_producto;
            }
            else
            {
                if (existente.cantidad < prStock.Stock)// Validamos que haya suficiente stock
                {
                    existente.cantidad++;
                    total += existente.precio_venta_producto;
                }
                else
                {
                    MostrarMensaje("No hay suficiente stock del producto seleccionado");
                }
            }

            Txt_TotalVenta.Text = total.ToString();
            dg_ProductosSeleecionados.Items.Refresh();// Es para forzar que se actualize la tabla
            dg_ProductosDisponibles.SelectedItem = null;
        }
        private void btn_RealizarVenta_Click(object sender, RoutedEventArgs e)
        {
            ConexionBD conexion = new ConexionBD();

            if(total == 0) {
                MostrarMensaje();
                return;
            }

            ResumenVenta resumenVenta = new ResumenVenta();
            resumenVenta.ShowDialog();

            if (!resumenVenta.cancelada)
                VaciarTablaSeleccionados();// No se vacia si se cancelo, por que podria ser para seleccionar o quitar productos

            CargarProductosDisponibles();
    }
        private void dg_ProductosSeleecionados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var prod = dg_ProductosSeleecionados.SelectedItem as ProductoSeleccionadoVenta;
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
        
            Txt_TotalVenta.Text = total.ToString();
            dg_ProductosSeleecionados.Items.Refresh();
        
            // Limpia selección para permitir nuevo clic
            dg_ProductosSeleecionados.SelectedItem = null;
        }
    }
}
