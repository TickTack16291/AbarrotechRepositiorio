using System;
using System.Windows;
using System.Windows.Media;

namespace LoginDeAbarrotech
{
    /// <summary>
    /// Lógica de interacción para Configuracion.xaml
    /// </summary>
    public partial class Configuracion : Window
    {
        private ConexionBD conexion;
        private string usuarioActual;

        public Configuracion()
        {
            InitializeComponent();
            conexion = new ConexionBD();
            usuarioActual = LoginAbarrotech.UsuarioGlobal;
            txt_CurrentUser.Text = $"Usuario: {usuarioActual}";
        }

        public void MostrarMensaje(string mensaje = null)
        {
            if (mensaje != null)
                Lbl_mensaje.Content = mensaje;

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
        public void VaciarCasillas()
        {
            txt_NuevoUsuario.Text = string.Empty;
            pwd_Actual.Password = string.Empty;
            pwd_Nueva.Password = string.Empty;
            pwd_Confirmar.Password = string.Empty;
        }

        private void btn_GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            Lbl_mensaje.Visibility = Visibility.Collapsed;

            try
            {
                string nuevoUsuario = null;
                string nuevaContrasena = null;
                bool hayUsuario = !string.IsNullOrWhiteSpace(txt_NuevoUsuario.Text);
                bool hayContrasena = !string.IsNullOrWhiteSpace(pwd_Actual.Password) ||
                                     !string.IsNullOrWhiteSpace(pwd_Nueva.Password) ||
                                     !string.IsNullOrWhiteSpace(pwd_Confirmar.Password);

                // Validar que al menos un campo tenga datos
                if (!hayUsuario && !hayContrasena)
                {
                    MostrarMensaje("No se realizaron cambios. Completa al menos un campo.");
                    return;
                }

                if (hayUsuario)
                {
                    nuevoUsuario = txt_NuevoUsuario.Text;

                    // Verificar que el usuario no exista ya (solo si es diferente al actual)
                    if (nuevoUsuario != usuarioActual && conexion.ValidarUsuario(nuevoUsuario))
                    {
                        MostrarMensaje("El nombre de usuario ya existe. Elige otro.");
                        txt_NuevoUsuario.Focus();
                        return;
                    }
                }

                if (hayContrasena)
                {
                    // Validar que todos los campos de contraseña estén llenos
                    if (string.IsNullOrWhiteSpace(pwd_Actual.Password) ||
                        string.IsNullOrWhiteSpace(pwd_Nueva.Password) ||
                        string.IsNullOrWhiteSpace(pwd_Confirmar.Password))
                    {
                        MostrarMensaje("Debes completar todos los campos de contraseña.");
                        pwd_Actual.Focus();
                        return;
                    }

                    // Verificar que la contraseña actual sea correcta
                    if (!conexion.ValidarContrasenaUsuario(usuarioActual, pwd_Actual.Password))
                    {
                        MostrarMensaje("La contraseña actual es incorrecta.");
                        pwd_Actual.Focus();
                        return;
                    }

                    // Verificar que las nuevas contraseñas coincidan
                    if (pwd_Nueva.Password != pwd_Confirmar.Password)
                    {
                        MostrarMensaje("Las nuevas contraseñas no coinciden.");
                        pwd_Nueva.Focus();
                        return;
                    }

                    nuevaContrasena = pwd_Nueva.Password;
                }

                long idUsuario = conexion.ObtenerIdUsuario(usuarioActual);

                if (conexion.ModificarUsuarioContrasena(idUsuario, nuevoUsuario, nuevaContrasena))
                {
                    if (nuevoUsuario != null)
                    {
                        LoginAbarrotech.UsuarioGlobal = nuevoUsuario;
                        usuarioActual = nuevoUsuario;
                        txt_CurrentUser.Text = $"Usuario: {usuarioActual}";
                    }

                    MostrarMensaje("Usuario/Contraseña modificado correctamente");

                    VaciarCasillas();
                }
                else
                {
                    MostrarMensaje("Error al guardar los cambios.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar cambios: {ex.Message}");
            }
        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}