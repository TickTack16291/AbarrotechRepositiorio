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
    /// Lógica de interacción para DatosProducto.xaml
    /// </summary>
    public partial class DatosProducto : Window
    {
        private Producto productoActual;
        public long codigoDeBarras;
        public DateTime fechaCaducidad;
        public DateTime fechaElaboracion;
        public bool cancelado = false;
        public DatosProducto(Producto producto)
        {
            InitializeComponent();
            productoActual = producto;

            txt_NombreProducto.Text = producto.nombre_producto;
        }

        public void MostrarMensaje(string mensaje = null)
        {

            if (mensaje != null)
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
        private void btn_Aceptar_Click(object sender, RoutedEventArgs e)
        {
            // Validar espacios vacios
            if (string.IsNullOrWhiteSpace(txt_CodigoBarras.Text) || !dp_FechaCaducidad.SelectedDate.HasValue || !dp_FechaElaboracion.SelectedDate.HasValue)
            {
                MostrarMensaje();
                txt_CodigoBarras.Focus();
                return;
            }

            // Validar que el código de barras sea numerico
            if (!long.TryParse(txt_CodigoBarras.Text, out long codigoBarras))
            {
                MostrarMensaje("El código de barras debe ser numérico");
                txt_CodigoBarras.Focus();
                return;
            }

            // Validar que la fecha de elaboración no sea futura
            if (dp_FechaElaboracion.SelectedDate.Value > DateTime.Now)
            {
                MostrarMensaje("La fecha de elaboración no puede ser futura");
                dp_FechaElaboracion.Focus();
                return;
            }

            // Validar que la fecha de caducidad sea posterior a la de elaboración
            if (dp_FechaCaducidad.SelectedDate.Value <= dp_FechaElaboracion.SelectedDate.Value)
            {
                MostrarMensaje("La fecha de caducidad debe ser despues a la de elaboración");
                dp_FechaCaducidad.Focus();
                return;
            }

            // Validar que el producto no esté ya caducado
            if (dp_FechaCaducidad.SelectedDate.Value < DateTime.Now)
            {
                MostrarMensaje("La fecha de caducidad debe ser futura");
                dp_FechaCaducidad.Focus();
                return;
            }

            codigoDeBarras = codigoBarras;// Esta variable se obtuvo al validar que el codigo de barras sea numerico
            fechaCaducidad = dp_FechaCaducidad.SelectedDate.Value;
            fechaElaboracion = dp_FechaElaboracion.SelectedDate.Value;

            this.Close();
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            cancelado = true;
            this.Close();
        }
    }
}