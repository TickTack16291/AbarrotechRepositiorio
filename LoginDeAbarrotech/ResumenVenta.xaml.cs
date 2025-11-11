using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static LoginDeAbarrotech.Ventas;

namespace LoginDeAbarrotech
{
    public partial class ResumenVenta : Window
    {
        public ResumenVenta()
        {
            InitializeComponent();
            CargarDatosDeVenta();
            CargarProductosVenta();
        }

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

            venta.id_venta = 0;
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
            if (conexion.RealizarVenta(venta))
            {
                MessageBox.Show("¡Venta realizada correctamente!");
            }
            else
            {
                MessageBox.Show("Error al registrar la venta.");
            }
            Hide();
        }
    }
}
