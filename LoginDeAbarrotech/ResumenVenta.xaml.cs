using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static LoginDeAbarrotech.Ventas;
using System.IO;

namespace LoginDeAbarrotech
{
    public partial class ResumenVenta : Window
    {
        /// <summary>
        /// Lógica de interacción para ResumenVenta.xaml
        /// </summary>
        public ResumenVenta()
        {
            InitializeComponent();
            CargarDatosDeVenta();
            CargarProductosVenta();
        }
        
        /// <summary>
        /// Objeto para pasar datos de la venta
        /// </summary>
        Venta venta = new Venta();
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
        public void CargarDatosDeVenta()
        {
            txt_usuario.Text = LoginAbarrotech.UsuarioGlobal;
            txt_FechaHora.Text = DateTime.Now.ToString("g");
            txt_caja.Text = "1";
            txt_distintos.Text = Ventas.productosSeleccionados.Count.ToString();
            txt_productosTotales.Text = Ventas.productosSeleccionados.Sum(p => p.cantidad).ToString();
            float total = Ventas.productosSeleccionados.Sum(p => p.Subtotal);
            txt_Total.Text = total.ToString();

            ConexionBD conexion = new ConexionBD();

            venta.id_venta = 0;// Chance deberia quitarlo de la clase, pero que mas da solo una linea
            venta.id_usuario = conexion.ObtenerIdUsuario(LoginAbarrotech.UsuarioGlobal);
            venta.fecha_venta = DateTime.Now;
            venta.total_venta = total;
            venta.caja = 1;
        }
        public void CargarProductosVenta()
        {
            dg_resumenVenta.ItemsSource = Ventas.productosSeleccionados;
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

            venta.forma_pago_venta = cb_metodoPago.Text.Trim();

            // Se recalculan por seguridad
            venta.total_venta = Ventas.productosSeleccionados.Sum(p => p.Subtotal);
            venta.fecha_venta = DateTime.Now;

            ConexionBD conexion = new ConexionBD();
            if (conexion.RealizarVenta(venta))// Aqui se hace la venta y se agrega a la base de datos
            {
                MessageBox.Show("¡Venta realizada correctamente!");
                CrearTicket(); // Se guardan en -> "C:\Users\bjrf8\OneDrive\TrabajosICBI\6to semestre\BDD\Tickets"


                // Agregamos los detalles de la venta necesarios(1 producto)
                foreach (var ps in Ventas.productosSeleccionados)
                    conexion.AgregarDetalleVenta(long.Parse(conexion.ObtenerIdVenta()), ps.id_producto, ps.cantidad, ps.precio_venta_producto);
                // falta hacer modificar el inventario y las trasnsacciones, pero debo hacer primero compras jaja que hueva

                // Inventario y transacción
                /// Campos de transacciones:
                /// long idInventario, long idInicioSesion, string tipoTransacción, int cantidadModificada, DateTime fechaRegistro, long idVenta, long idCompra
                foreach (var ps in Compras.productosSeleccionados)
                {
                    // Modificamos el inventario con procedure
                    conexion.RealizarVentaPorProcedure(ps.id_producto, ps.cantidad);

                    // Id de la sesion más reciente
                    long idSesion = conexion.ObtenerIdSesionMasReciente(conexion.ObtenerIdUsuario(LoginAbarrotech.UsuarioGlobal));
                    //Id del inventario recien agregado
                    long idInventario = conexion.ObtenerIdInventario();
                    //Id de la compra
                    long idCompra = conexion.ObtenerIdCompra();

                    // Agregamos la transacción
                    conexion.RealizarTransaccion(idInventario, idSesion, "Venta", ps.cantidad, DateTime.Now, null, idCompra);
                }

                // Limpiamos la lista de productos seleccionados para mas ventas
                Ventas.productosSeleccionados.Clear();
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
            string idVenta = conexion.ObtenerIdVenta();// Obtiene el ID de la venta más reciente
            venta.forma_pago_venta = cb_metodoPago.Text.Trim();

            // Contenido inicial del ticket

            // Usamos un foreach para hacer la lista de productos
            string listaProductos = "";
            foreach (var ps in Ventas.productosSeleccionados)
            {// Agregamos todos los productos seleccionados para la venta a un string
                listaProductos += $"{ps.nombre_producto} - Cantidad: {ps.cantidad} - Precio Unitario: {ps.precio_venta_producto} - Subtotal: {ps.Subtotal}\n";
            }

            string tiket = $"- - - - Ticket de Venta No. {idVenta} - - - -\n" +
                           $"Usuario: {LoginAbarrotech.UsuarioGlobal}\n" +
                           $"Total: {venta.total_venta}\n" +
                           $"Caja: {venta.caja}\n" +
                           $"Fecha y hora: {venta.fecha_venta}\n" +
                           $"Método de pago: {venta.forma_pago_venta}\n" +
                           "---------------------------------------------------\n" +
                           "Productos:\n" + listaProductos;

            // Ruta donde se guardarán los tickets, creo que deberiamos usar una general para que no falle en otros dispositivos
            string targetDir = @"C:\Users\bjrf8\OneDrive\TrabajosICBI\6to semestre\BDD\Tickets\Ventas";

            try
            {
                // Asegura que el directorio exista (lo crea si falta).
                Directory.CreateDirectory(targetDir);

                // El "$" es como la "f" en python
                string fileName = $"ticketVenta{idVenta}_{LoginAbarrotech.UsuarioGlobal}.txt"; // El ticket lleva el id de la venta y el usuario que la realizó

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
