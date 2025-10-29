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
    /// Lógica de interacción para LoginAbarrotech.xaml
    /// </summary>
    public partial class LoginAbarrotech : Window
    {
        public LoginAbarrotech()
        {
            InitializeComponent();
            txtUsuario.Focus();
        }
        public static string UsuarioGlobal;
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Lbl_error.Visibility = Visibility.Hidden;

            string usuarioAux = txtUsuario.Text;
            string contrasenaAux = txtPassword.Password;

            if (usuarioAux == "" || contrasenaAux == "")// Campos vacios
            {
                Lbl_error.Content = "No puede haber campos vacios";
                Lbl_error.Visibility = Visibility.Visible;
                txtUsuario.Focus();
                var animacion = new System.Windows.Media.Animation.ThicknessAnimation();
                animacion.Duration = TimeSpan.FromMilliseconds(100);
                animacion.From = new Thickness(0);
                animacion.To = new Thickness(5);
                animacion.AutoReverse = true;
                animacion.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(2);
                Lbl_error.BeginAnimation(MarginProperty, animacion);
            }
            else
            {
                ConexionBD conexion = new ConexionBD();

                bool aux = conexion.validar_inicio_sesion(usuarioAux, contrasenaAux);
                if (aux)
                {
                    // Aqui debe abrir un el menu de operciones

                    // Operaciones necesarias para registrar el inicio de sesion
                    int idAux = conexion.ObtenerIdUsuario(usuarioAux);
                    DateTime fechaActual= DateTime.Now;
                    conexion.RegistrarInicioSesion(idAux, fechaActual, 1);

                    LoginAbarrotech.UsuarioGlobal = usuarioAux;

                    MenuOperaciones menuoperaciones = new MenuOperaciones();

                    menuoperaciones.Show();
                    this.Hide();
                }
                else
                {
                    Lbl_error.Content = "Usuario o contraseña invalidos";
                    Lbl_error.Visibility = Visibility.Visible;
                    txtUsuario.Focus();
                    var animacion = new System.Windows.Media.Animation.ThicknessAnimation();
                    animacion.Duration = TimeSpan.FromMilliseconds(100);
                    animacion.From = new Thickness(0);
                    animacion.To = new Thickness(5);
                    animacion.AutoReverse = true;
                    animacion.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(2);
                    Lbl_error.BeginAnimation(MarginProperty, animacion);
                }
            }
        }
        private void btn_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
