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
    /// Lógica de interacción para RegistroUsuarios.xaml
    /// </summary>
    public partial class RegistroUsuarios : Window
    {
        public RegistroUsuarios()
        {
            InitializeComponent();
        }
        public void MostrarMensaje(string mensaje)
        {
            Lbl_mensaje.Foreground = Brushes.Green;

            if (mensaje != "Usuario agregado correctamente")
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
            ct_IdEmpleado.Text = string.Empty;
            ct_Usuario.Text = string.Empty;
            ct_Contrasena.Password = string.Empty;
            cb_RolUsuario.Text = string.Empty;
        }
        public bool ValidarUsuariosRepetidos(Usuario ingresado)
        {
            ConexionBD conexion = new ConexionBD();
            var usuarios = conexion.ObtenerUsuarios();
            foreach (var u in usuarios)
            {
                if (
                    u.id_empleado == ingresado.id_empleado &&
                    u.usuario == ingresado.usuario &&
                    u.rol_usuario == ingresado.rol_usuario &&
                    u.contrasena == ingresado.contrasena
                )
                    return true; // Hay un empleado exactamente igual
            }
            return false; // No hay empleados exactamente iguales
        }
        private void btn_Cancelar_Click(object sender, RoutedEventArgs e) {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Hide();
        }
        private void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(ct_IdEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_Usuario.Text) ||
                string.IsNullOrWhiteSpace(ct_Contrasena.Password) ||
                cb_RolUsuario.SelectedItem == null)
            {
                MostrarMensaje("No puede haber campos vacios");
                return;
            }
            // Validar que el ID de empleado sea un número entero 
            if (!int.TryParse(ct_IdEmpleado.Text, out int idEmpleado))
            {
                MostrarMensaje("El id de empleado debe ser numerico");
                return;
            }

            // Verificar si el usuario ya existe
            ConexionBD conexion = new ConexionBD();

            // Crear objeto Usuario
            Usuario nuevoUsuario = new Usuario(
                0, 
                idEmpleado, 
                ct_Usuario.Text, 
                ct_Contrasena.Password, 
                cb_RolUsuario.Text
                );

            if (ValidarUsuariosRepetidos(nuevoUsuario))
            {
                MostrarMensaje("El usuario ya existe en la base de datos");
                return;
            }

            // Intentar insertar en la base de datos
            if (conexion.ingresar_usuarios(nuevoUsuario))
            {
                MostrarMensaje("Usuario agregardo a la base de datos");
                MenuOperaciones menuOperaciones = new MenuOperaciones();
                menuOperaciones.Show();
                this.Hide();
            }
            else
            {
                MostrarMensaje("No se puedo agregar el usuario a la base de datos");
            }
        }
    }
}
