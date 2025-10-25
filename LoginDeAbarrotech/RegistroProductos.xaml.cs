using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// Lógica de interacción para RegistroProductos.xaml
    /// </summary>
    public partial class RegistroProductos : Window
    {
        public RegistroProductos()
        {
            InitializeComponent();
            CargarProductos();
            CargarNombresProvedores();
        }
        public void MostrarMensaje(string mensaje)
        {
            Lbl_mensaje.Foreground = Brushes.Green;

            if (mensaje != "Producto agregado correctamente" || mensaje != "Producto modificado correctamente")
                Lbl_mensaje.Foreground = Brushes.Red;

            Lbl_mensaje.Content = mensaje;
            Lbl_mensaje.Visibility = Visibility.Visible;
            var margenBase = Lbl_mensaje.Margin;
            var animacion = new System.Windows.Media.Animation.ThicknessAnimation();
            animacion.From = margenBase;
            animacion.To = new Thickness(margenBase.Left + 5, margenBase.Top, margenBase.Right, margenBase.Bottom);
            animacion.Duration = TimeSpan.FromMilliseconds(100);
            animacion.AutoReverse = true;
            animacion.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(2);
            Lbl_mensaje.BeginAnimation(MarginProperty, animacion);
        }
        public void VaciasCasillas() {
            ct_Nombre.Text = string.Empty;
            ct_Marca.Text = string.Empty;
            cb_Categoria.Text = string.Empty;
            ct_Presentacion.Text = string.Empty;
            cb_UnidadMedida.Text = string.Empty;
            ct_PrecioCompra.Text = string.Empty;
            ct_PrecioVenta.Text = string.Empty;
            cb_Categoria.Text= string.Empty;

        }
        public void CargarProductos() {
            ConexionBD conexion = new ConexionBD();
            var productos = conexion.ObtenerProductos();
            dg_Productos.ItemsSource = productos;
        }
        public void CargarNombresProvedores()
        {
            ConexionBD conexion = new ConexionBD();
            var Ps = conexion.ObtenerNombresProveedores();
            cb_nombreProvedor.Items.Clear();
            foreach (var P in Ps) {
                cb_nombreProvedor.Items.Add(P);
            }
        }
        public bool ValidarProductosRepetidos(Producto Ingresado)
        {
            ConexionBD conexion = new ConexionBD();
            var productos = conexion.ObtenerProductos();
            foreach (var p in productos)
            {
                if (
                    p.nombre_producto == Ingresado.nombre_producto &&
                    p.marca_producto == Ingresado.marca_producto &&
                    p.presentacion_producto == Ingresado.presentacion_producto &&
                    p.unidad_medida_producto == Ingresado.unidad_medida_producto
                )
                    return true; // Hay un producto exactamente igual
            }
            return false; // No hay productos exactamente iguales
        }
        private void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Verificamos que ningún campo esté vacío
            if (string.IsNullOrWhiteSpace(ct_Nombre.Text) ||
                string.IsNullOrWhiteSpace(ct_Marca.Text) ||
                string.IsNullOrWhiteSpace(cb_Categoria.Text) ||
                string.IsNullOrWhiteSpace(ct_Presentacion.Text) ||
                string.IsNullOrWhiteSpace(cb_UnidadMedida.Text) ||
                string.IsNullOrWhiteSpace(ct_PrecioCompra.Text) ||
                string.IsNullOrWhiteSpace(ct_PrecioVenta.Text))
            {
                // Se muestra si hay campos vacios
                MostrarMensaje("No puede haber campos vacios");
                return;
            }

            // Validar que los precios sean números
            if (!float.TryParse(ct_PrecioVenta.Text, out float precioVenta))// Intenta convertir el texto a flotante, si funciona el valor se guarda en la variable
            {
                MostrarMensaje("El precio de venta debe ser un número válido.");
                return;
            }
            if (!float.TryParse(ct_PrecioCompra.Text, out float precioCompra))// Lo mismo que el de arriba
            {
                MostrarMensaje("El precio de compra debe ser un número válido");
                return;
            }

            // Crear el objeto Producto con los datos del formulario

            ConexionBD conexion = new ConexionBD();

            int idProAux = conexion.ObtenerIdProveedor(cb_nombreProvedor.Text);

            Producto producto = new Producto(0,
                ct_Nombre.Text,
                ct_Marca.Text,
                ct_Presentacion.Text,
                cb_UnidadMedida.Text,
                precioVenta,// La variable que se deberia haber guardado
                precioCompra,//x2
                1, // estado
                cb_Categoria.Text,
                idProAux
            );

            if (ValidarProductosRepetidos(producto)){
                MostrarMensaje("No se permiten los productos repetidos");
                return;
            }

            if (conexion.AgregarProducto(producto))
            {
                // Muestra si se agrego correctamente el producto
                MostrarMensaje("Producto agregado correctamente");
                VaciasCasillas();
                ct_Nombre.Focus();
                CargarProductos();
            }
            else
            {
                // Muestra si no se agrego correctamente el producto
                MostrarMensaje("No se pudo agregar el producto");
            }
        }
        private void btn_Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (dg_Productos.SelectedItem is Producto productoSeleccionado)
            {
                // Verificamos que ningún campo esté vacío
                if (string.IsNullOrWhiteSpace(ct_Nombre.Text) ||
                    string.IsNullOrWhiteSpace(ct_Marca.Text) ||
                    string.IsNullOrWhiteSpace(cb_Categoria.Text) ||
                    string.IsNullOrWhiteSpace(ct_Presentacion.Text) ||
                    string.IsNullOrWhiteSpace(cb_UnidadMedida.Text) ||
                    string.IsNullOrWhiteSpace(ct_PrecioCompra.Text) ||
                    string.IsNullOrWhiteSpace(ct_PrecioVenta.Text))
                {
                    MostrarMensaje("No puede haber campos vacios");
                    return;
                }

                if (!float.TryParse(ct_PrecioVenta.Text, out float precioVenta))
                {
                    MostrarMensaje("El precio de venta debe ser un número válido.");
                    return;
                }
                if (!float.TryParse(ct_PrecioCompra.Text, out float precioCompra))
                {
                    MostrarMensaje("El precio de compra debe ser un número válido");
                    return;
                }

                // Usar el ID del producto seleccionado

                ConexionBD conexion = new ConexionBD();

                int idProAux = conexion.ObtenerIdProveedor(cb_nombreProvedor.Text);

                Producto productoModificado = new Producto(
                    productoSeleccionado.id_producto,
                    ct_Nombre.Text,
                    ct_Marca.Text,
                    ct_Presentacion.Text,
                    cb_UnidadMedida.Text,
                    precioVenta,
                    precioCompra,
                    productoSeleccionado.estado_producto, // Mantener el estado original
                    cb_Categoria.Text,
                    idProAux
                );

                //if (ValidarProductosRepetidos(productoModificado))
                //{
                //    MostrarMensaje("No se permiten los productos repetidos");
                //    return;
                //}

                if (conexion.ModificarProducto(productoModificado))
                {
                    MostrarMensaje("Producto modificado correctamente");
                    VaciasCasillas();
                    ct_Nombre.Focus();
                    CargarProductos();
                }
                else
                {
                    MostrarMensaje("No se pudo modificar el producto");
                }
            }
            else
            {
                MostrarMensaje("Seleccione un producto de la lista para modificar");
            }
        }
        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
        }
        private void dg_Productos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConexionBD conexion = new ConexionBD();


            if (dg_Productos.SelectedItem is Producto producto)
            {
                string proAux = conexion.ObtenerNombreProveedor(producto.id_proveedor_producto);

                ct_Nombre.Text = producto.nombre_producto;
                ct_Marca.Text = producto.marca_producto;
                cb_Categoria.Text = producto.categoria_producto;
                ct_Presentacion.Text = producto.presentacion_producto;
                cb_UnidadMedida.Text = producto.unidad_medida_producto.ToString();
                ct_PrecioCompra.Text = producto.precio_compra_producto.ToString();
                ct_PrecioVenta.Text = producto.precio_venta_producto.ToString();
                cb_nombreProvedor.Text = proAux;
            }
        }
    }
}
