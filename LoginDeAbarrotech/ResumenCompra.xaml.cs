using System;
using System.Collections.Generic;
using System.IO;
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

namespace LoginDeAbarrotech
{
    /// <summary>
    /// Lógica de interacción para ResumenCompra.xaml
    /// </summary>
    public partial class ResumenCompra : Window
    {
        public ResumenCompra()
        {
            InitializeComponent();
            CargarDatosDeCompra();
            CargarProductosCompra();
        }

        /// <summary>
        /// Objeto para pasar datos de la compra
        /// </summary>
        Compra compra = new Compra();
        public bool cancelada = false;
        public void MostrarMensaje()
        {
            Lbl_mensaje.Visibility = Visibility.Visible;
            var margenBase = Lbl_mensaje.Margin;
            var animacion = new System.Windows.Media.Animation.ThicknessAnimation
            {
                From = margenBase,
                To = new Thickness(margenBase.Left + 5, margenBase.Top, margenBase.Right, margenBase.Bottom),
                Duration = TimeSpan.FromMilliseconds(100),
                AutoReverse = true,
                RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(2)
            };
            Lbl_mensaje.BeginAnimation(MarginProperty, animacion);
        }
        public void CargarDatosDeCompra()
        {
            txt_usuario.Text = LoginAbarrotech.UsuarioGlobal;
            txt_FechaHora.Text = DateTime.Now.ToString("g");
            txt_caja.Text = "1";
            txt_distintos.Text = Compras.productosSeleccionados.Count.ToString();
            txt_productosTotales.Text = Compras.productosSeleccionados.Sum(p => p.cantidad).ToString();
            float total = Compras.productosSeleccionados.Sum(p => p.Subtotal);
            txt_Total.Text = total.ToString();

            ConexionBD conexion = new ConexionBD();

            compra.id_compra = 0;// Chance deberia quitarlo de la clase, pero que mas da solo una linea
            compra.id_usuario = conexion.ObtenerIdUsuario(LoginAbarrotech.UsuarioGlobal);
            compra.fecha_compra = DateTime.Now;
            compra.total_compra = total;
            compra.caja = 1;
        }
        public void CargarProductosCompra()
        {
            dg_resumenVenta.ItemsSource = Compras.productosSeleccionados;
        }
        private void Button_Click_Cancelar(object sender, RoutedEventArgs e)
        {
            cancelada = true;
            Hide();
        }
        private void Button_Click_Confirmar(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cb_metodoPago.Text))
            {
                MostrarMensaje();
                return;
            }

            compra.forma_pago_compra = cb_metodoPago.Text.Trim();

            // Se recalculan por seguridad
            compra.total_compra = Compras.productosSeleccionados.Sum(p => p.Subtotal);
            compra.fecha_compra = DateTime.Now;

            ConexionBD conexion = new ConexionBD();
            if (conexion.RealizarCompra(compra))// Aqui se hace la venta y se agrega a la base de datos
            {
                MessageBox.Show("¡Venta realizada correctamente!");
                CrearTicket(); // Se guardan en -> "C:\Users\bjrf8\OneDrive\TrabajosICBI\6to semestre\BDD\Tickets"

                // Agregamos los detalles de la compra necesarios
                foreach (var ps in Compras.productosSeleccionados)
                    conexion.AgregarDetalleCompra(conexion.ObtenerIdCompra(), ps.id_producto, ps.cantidad, ps.precio_compra_producto);

                // Inventario y transacción
                /// Campos de transacciones:
                /// long idInventario, long idInicioSesion, string tipoTransacción, int cantidadModificada, DateTime fechaRegistro, long idVenta, long idCompra
                foreach (var ps in Compras.productosSeleccionados)
                {
                    // Agregamos el inventario
                    conexion.AgregarInventario(ps.id_producto, 0, ps.cantidad, "Almacén", DateTime.Now, DateTime.Now.AddMonths(2));// Hay que checar lo de las fechas de elaboración y caducidad

                    // Id de la sesion más reciente
                    long idSesion = conexion.ObtenerIdSesionMasReciente(conexion.ObtenerIdUsuario(LoginAbarrotech.UsuarioGlobal));
                        //Id del inventario recien agregado
                        long idInventario = conexion.ObtenerIdInventario();
                        //Id de la compra
                        long idCompra = conexion.ObtenerIdCompra();

                    // Agregamos la transacción
                    conexion.RealizarTransaccion(idInventario, idSesion, "Compra", ps.cantidad, DateTime.Now, null, idCompra);
                }

                // Limpiamos la lista de productos seleccionados para mas compras
                Compras.productosSeleccionados.Clear();

            }
            else
            {
                MessageBox.Show("Error al registrar la venta.");
            }
            Hide();
        }

        /// <summary>
        /// Majeno de archivos para tikets
        /// </summary>
        private void CrearTicket()
        {
            ConexionBD conexion = new ConexionBD();
            long idCompra = conexion.ObtenerIdCompra();// Obtiene el ID de la venta más reciente
            compra.forma_pago_compra = cb_metodoPago.Text.Trim();

            // Contenido inicial del ticket

            // Usamos un foreach para hacer la lista de productos
            string listaProductos = "";
            foreach (var ps in Compras.productosSeleccionados)
            {// Agregamos todos los productos seleccionados para la venta a un string
                listaProductos += $"{ps.nombre_producto} - Cantidad: {ps.cantidad} - Precio Unitario: {ps.precio_compra_producto} - Subtotal: {ps.Subtotal}\n";
            }

            string tiket = $"- - - - Ticket de Compra No. {idCompra} - - - -\n" +
                           $"Usuario: {LoginAbarrotech.UsuarioGlobal}\n" +
                           $"Total: {compra.total_compra}\n" +
                           $"Caja: {compra.caja}\n" +
                           $"Fecha y hora: {compra.fecha_compra}\n" +
                           $"Método de pago: {compra.forma_pago_compra}\n" +
                           "---------------------------------------------------\n" +
                           "Productos:\n" + listaProductos;

            // Ruta donde se guardarán los tickets, creo que deberiamos usar una general para que no falle en otros dispositivos
            string targetDir = @"C:\Users\bjrf8\OneDrive\TrabajosICBI\6to semestre\BDD\Tickets\Compras";

            try
            {
                // Asegura que el directorio exista (lo crea si falta).
                Directory.CreateDirectory(targetDir);

                // El "$" es como la "f" en python
                string fileName = $"ticket_Compra{idCompra}_{LoginAbarrotech.UsuarioGlobal}.txt"; // El ticket lleva el id de la compra y el usuario que la realizó

                // Combina de forma segura el directorio y el nombre de archivo.
                string fullPath = Path.Combine(targetDir, fileName);

                // Escribe todo el contenido del ticket en el archivo (sobrescribe si existe).
                File.WriteAllText(fullPath, tiket);

                MessageBox.Show($"¡Ticket creado con éxito en:\n{fullPath}", "Ticket creado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el ticket:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
