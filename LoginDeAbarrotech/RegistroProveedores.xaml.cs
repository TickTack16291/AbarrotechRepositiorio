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
    /// Lógica de interacción para RegistroProveedores.xaml
    /// </summary>
    public partial class RegistroProveedores : Window
    {
        public int idProvAux = 0;
        
        public RegistroProveedores()
        {
            InitializeComponent();
            CargarProveedores();
        }
        public void MostrarMensaje(string mensaje)
        {
            Lbl_mensaje.Foreground = Brushes.Green;

            if (mensaje != "Proveedor agregado correctamente" || mensaje != "Proveedor modificado correctamente")
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
        public void VaciasCasillas()
        {
            ct_NombreProveedor.Text = string.Empty;
            ct_ResponsableProveedor.Text = string.Empty;
            ct_DireccionProveedor.Text = string.Empty;
            ct_EmailProveedor.Text = string.Empty;
            ct_TelefonoProveedor.Text = string.Empty;
            cb_EstadoProveedor.Text = string.Empty;
        }
        public void CargarProveedores()
        {
            ConexionBD conexion = new ConexionBD();
            var provedores = conexion.ObtenerProveedores();// Cambiar jaja
            dg_Proveedores.ItemsSource = provedores;
        }
        public bool ValidarProveedoresRepetidos(Proveedor Ingresado)
        {
            ConexionBD conexion = new ConexionBD();
            var proveedores = conexion.ObtenerProveedores();
            foreach (var p in proveedores)
            {
                if (
                    p.nombre_proveedor == Ingresado.nombre_proveedor &&
                    p.email_proveedor == Ingresado.email_proveedor &&
                    p.telefono_proveedor == Ingresado.telefono_proveedor
                )
                    return true; // Hay un proveedor con algunos datos iguales
            }
            return false; // No hay proveedores iguales
        }
        private void dg_Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dg_Proveedores.SelectedItem is Proveedor proveedor)
            {
                idProvAux = proveedor.id_proveedor;

                ct_NombreProveedor.Text = proveedor.nombre_proveedor;
                ct_ResponsableProveedor.Text = proveedor.responsable_proveedor;
                ct_DireccionProveedor.Text = proveedor.direccion_proveedor;
                ct_TelefonoProveedor.Text = proveedor.telefono_proveedor.ToString();
                ct_EmailProveedor.Text = proveedor.email_proveedor;
                cb_EstadoProveedor.Text = proveedor.estado_proveedor;
            }
        }
        private void btn_Cancelar_Click(object sender, RoutedEventArgs e) {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Close();
        }
        private void btn_Guardar_Click(object sender, RoutedEventArgs e) {
            // Verificamos que ningún campo esté vacío
            if (string.IsNullOrWhiteSpace(ct_NombreProveedor.Text) ||
                string.IsNullOrWhiteSpace(ct_ResponsableProveedor.Text) ||
                string.IsNullOrWhiteSpace(ct_DireccionProveedor.Text) ||
                string.IsNullOrWhiteSpace(ct_EmailProveedor.Text) ||
                string.IsNullOrWhiteSpace(ct_TelefonoProveedor.Text) ||
                string.IsNullOrWhiteSpace(cb_EstadoProveedor.Text))
            {
                // Se muestra si hay campos vacios
                MostrarMensaje("No puede haber campos vacios");
                return;
            }

            // Validar que los precios sean números
            if (!long.TryParse(ct_TelefonoProveedor.Text, out long telefonoProveedor))// Intenta convertir el texto a int, si funciona el valor se guarda en la variable
            {
                MostrarMensaje("El telefono debe ser un número válido.");
                return;
            }

            long longitud = telefonoProveedor.ToString().Length;

            if (longitud != 10)
            {
               MostrarMensaje("El telefono debe tener 10 digitos.");
            }

            // Crear el objeto Producto con los datos del formulario
            Proveedor proveedor = new Proveedor(
                0,
                ct_NombreProveedor.Text,
                ct_ResponsableProveedor.Text,
                ct_DireccionProveedor.Text,
                telefonoProveedor,// La variable que se deberia haber guardado
                ct_EmailProveedor.Text,
                cb_EstadoProveedor.Text
                );

            if (ValidarProveedoresRepetidos(proveedor))
            {
                MostrarMensaje("No se permiten los proveedores repetidos");
                return;
            }

            ConexionBD conexion = new ConexionBD();

            if (conexion.AgregarProveedor(proveedor))
            {
                // Muestra si se agrego correctamente el producto
                MostrarMensaje("Proveedor agregado correctamente");
                VaciasCasillas();
                ct_NombreProveedor.Focus();
                CargarProveedores();
            }
            else
            {
                // Muestra si no se agrego correctamente el proveedor
                MostrarMensaje("No se pudo agregar el proveedor");
            }
        }
        private void btn_Modificar_Click(object sender, RoutedEventArgs e) {
            // Verificamos que ningún campo esté vacío
            if (string.IsNullOrWhiteSpace(ct_NombreProveedor.Text) ||
                string.IsNullOrWhiteSpace(ct_ResponsableProveedor.Text) ||
                string.IsNullOrWhiteSpace(ct_DireccionProveedor.Text) ||
                string.IsNullOrWhiteSpace(ct_EmailProveedor.Text) ||
                string.IsNullOrWhiteSpace(ct_TelefonoProveedor.Text) ||
                string.IsNullOrWhiteSpace(cb_EstadoProveedor.Text))
            {
                // Se muestra si hay campos vacios
                MostrarMensaje("No puede haber campos vacios");
                return;
            }

            // Validar que los precios sean números
            if (!long.TryParse(ct_TelefonoProveedor.Text, out long telefonoProveedor))// Intenta convertir el texto a flotante, si funciona el valor se guarda en la variable
            {
                MostrarMensaje("El telefono debe ser un número válido.");
                return;
            }

            long longitud = telefonoProveedor.ToString().Length;

            if (longitud != 10)
            {
                MostrarMensaje("El telefono debe tener 10 digitos.");
                return;
            }

            // Crear el objeto Producto con los datos del formulario
            Proveedor proveedorModificado = new Proveedor(
                idProvAux,
                ct_NombreProveedor.Text,
                ct_ResponsableProveedor.Text,
                ct_DireccionProveedor.Text,
                telefonoProveedor,// La variable que se deberia haber guardado
                ct_EmailProveedor.Text,
                cb_EstadoProveedor.Text
                );

            //if (ValidarProveedoresRepetidos(proveedorModificado))
            //{
            //    MostrarMensaje("No se permiten los proveedores repetidos");
            //    return;
            //}

            ConexionBD conexion = new ConexionBD();

            if (conexion.ModificarProveedor(proveedorModificado))
            {
                // Muestra si se agrego correctamente el producto
                MostrarMensaje("Proveedor modificado correctamente");
                VaciasCasillas();
                ct_NombreProveedor.Focus();
                CargarProveedores();
            }
            else
            {
                // Muestra si no se agrego correctamente el proveedor
                MostrarMensaje("No se pudo modificar el proveedor");
            }
        }
    }
}
