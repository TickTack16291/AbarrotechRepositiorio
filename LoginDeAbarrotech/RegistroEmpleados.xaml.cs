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
    /// Lógica de interacción para RegistroEmpleados.xaml
    /// </summary>
    public partial class RegistroEmpleados : Window
    {
        public RegistroEmpleados()
        {
            InitializeComponent();
            CargarEmpleados();
        }
        public void MostrarMensaje(string mensaje)
        {
            Lbl_mensaje.Foreground = Brushes.Green;

            if (mensaje != "Empleado agregado correctamente" || mensaje != "Empleado modificado correctamente")
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
            ct_NombreEmpleado.Text = string.Empty;
            ct_RolEmpleado.Text = string.Empty;
            ct_DireccionEmpleado.Text = string.Empty;
            ct_TelefonoEmpleado.Text = string.Empty;
            ct_CorreoEmpleado.Text = string.Empty;
            ct_SalarioEmpleado.Text = string.Empty;
            cb_EstadoEmpleado.Text = string.Empty;
            ct_HoraEntrada.Text = string.Empty;
            ct_HoraSalida.Text = string.Empty;
            dp_FechaContrato.Text = string.Empty;
            dp_FechaFinContrato.Text = string.Empty;
        }
        public void CargarEmpleados()
        {
            ConexionBD conexion = new ConexionBD();
            var empleados = conexion.ObtenerEmpleados();
            dg_Empleados.ItemsSource = empleados;
        }
        public bool ValidarEmpleadosRepetidos(Empleado ingresado)
        {
            ConexionBD conexion = new ConexionBD();
            var empleados = conexion.ObtenerEmpleados();
            foreach (var e in empleados)
            {
                if (
                    e.nombre_empleado == ingresado.nombre_empleado &&
                    e.telefono_empleado == ingresado.telefono_empleado &&
                    e.correo_electronico_empleado == ingresado.correo_electronico_empleado
                )
                    return true; // Hay un empleado igual
            }
            return false; // No hay empleados iguales
        }
        private void dg_Empleados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg_Empleados.SelectedItem is Empleado empleado)
            {
                ct_NombreEmpleado.Text = empleado.nombre_empleado;
                ct_RolEmpleado.Text = empleado.rol_empleado;
                ct_DireccionEmpleado.Text = empleado.direccion_empleado;
                ct_TelefonoEmpleado.Text = empleado.telefono_empleado.ToString();
                ct_CorreoEmpleado.Text = empleado.correo_electronico_empleado;
                ct_SalarioEmpleado.Text = empleado.salario_empleado.ToString();

                cb_EstadoEmpleado.Text = empleado.estado_empleado;
                
                ct_HoraEntrada.Text = empleado.hora_entrada_empleado.ToString(@"hh\:mm");
                ct_HoraSalida.Text = empleado.hora_salida_empleado.ToString(@"hh\:mm");
                dp_FechaContrato.SelectedDate = empleado.fecha_inicio_contrato;
                dp_FechaFinContrato.SelectedDate = empleado.fecha_fin_contrato;
            }
        }
        private void btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Close();
        }
        private void btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Verificamos que ningún campo esté vacío
            if (string.IsNullOrWhiteSpace(ct_NombreEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_RolEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_DireccionEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_CorreoEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_TelefonoEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_SalarioEmpleado.Text) ||
                string.IsNullOrWhiteSpace(cb_EstadoEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_HoraEntrada.Text) ||
                string.IsNullOrWhiteSpace(ct_HoraSalida.Text) ||
                dp_FechaContrato.SelectedDate == null ||
                dp_FechaFinContrato.SelectedDate == null)
            {
                // Se muestra si hay campos vacios
                MostrarMensaje("No puede haber campos vacios");
                return;
            }
            // Validar que el teléfono sea un número válido
            if (!long.TryParse(ct_TelefonoEmpleado.Text, out long telefonoEmpleado))
            {
                MostrarMensaje("El teléfono debe ser un número válido.");
                return;
            }

            long longitud = telefonoEmpleado.ToString().Length;

            if (longitud != 10)
            {
                MostrarMensaje("El telefono debe tener 10 digitos.");
                return;
            }
            // Validar que el salario sea un número válido
            if (!float.TryParse(ct_SalarioEmpleado.Text, out float salarioEmpleado))
            {
                MostrarMensaje("El salario debe ser un número válido.");
                return;
            }
            // Validar que las horas sean válidas
            if (!TimeSpan.TryParse(ct_HoraEntrada.Text, out TimeSpan horaEntrada))
            {
                MostrarMensaje("La hora de entrada debe ser una hora válida.");
                return;
            }
            if (!TimeSpan.TryParse(ct_HoraSalida.Text, out TimeSpan horaSalida))
            {
                MostrarMensaje("La hora de salida debe ser una hora válida.");
                return;
            }

            // Crear el objeto Empleado con los datos del formulario
            Empleado empleado = new Empleado(
                0, // id_empleado, probablemente autoincrementable
                ct_NombreEmpleado.Text,
                ct_DireccionEmpleado.Text,
                telefonoEmpleado,
                ct_CorreoEmpleado.Text,
                ct_RolEmpleado.Text,
                salarioEmpleado,
                dp_FechaContrato.SelectedDate.Value,
                horaEntrada,
                horaSalida,
                cb_EstadoEmpleado.Text,
                dp_FechaFinContrato.SelectedDate.Value
            );

            if (ValidarEmpleadosRepetidos(empleado))
            {
                MostrarMensaje("No se permiten empleados repetidos");
                return;
            }

            ConexionBD conexion = new ConexionBD();

            if (conexion.AgregarEmpleado(empleado))
            {
                // Muestra si se agregó correctamente el empleado
                MostrarMensaje("Empleado agregado correctamente");
                VaciasCasillas();
                ct_NombreEmpleado.Focus();
                CargarEmpleados();
            }
            else
            {
                // Muestra si no se agregó correctamente el empleado
                MostrarMensaje("No se pudo agregar el empleado");
            }
        }
        private void btn_Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (dg_Empleados.SelectedItem is Empleado empleadoSeleccionado)
            {
                // Verificamos que ningún campo esté vacío
                if (string.IsNullOrWhiteSpace(ct_NombreEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_RolEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_DireccionEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_CorreoEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_TelefonoEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_SalarioEmpleado.Text) ||
                string.IsNullOrWhiteSpace(cb_EstadoEmpleado.Text) ||
                string.IsNullOrWhiteSpace(ct_HoraEntrada.Text) ||
                string.IsNullOrWhiteSpace(ct_HoraSalida.Text) ||
                dp_FechaContrato.SelectedDate == null ||
                dp_FechaFinContrato.SelectedDate == null)
                {
                    // Se muestra si hay campos vacios
                    MostrarMensaje("No puede haber campos vacios");
                    return;
                }
                // Validar que el teléfono sea un número válido
                if (!long.TryParse(ct_TelefonoEmpleado.Text, out long telefonoEmpleado))
                {
                    MostrarMensaje("El teléfono debe ser un número válido.");
                    return;
                }

                long longitud = telefonoEmpleado.ToString().Length;

                if (longitud != 10)
                {
                    MostrarMensaje("El telefono debe tener 10 digitos.");
                    return;
                }

                // Validar que el salario sea un número válido
                if (!float.TryParse(ct_SalarioEmpleado.Text, out float salarioEmpleado))
                {
                    MostrarMensaje("El salario debe ser un número válido.");
                    return;
                }
                // Validar que las horas sean válidas
                if (!TimeSpan.TryParse(ct_HoraEntrada.Text, out TimeSpan horaEntrada))
                {
                    MostrarMensaje("La hora de entrada debe ser una hora válida.");
                    return;
                }
                if (!TimeSpan.TryParse(ct_HoraSalida.Text, out TimeSpan horaSalida))
                {
                    MostrarMensaje("La hora de salida debe ser una hora válida.");
                    return;
                }

                // Crear el objeto Empleado con los datos del formulario
                Empleado empleadoModificado = new Empleado(
                    empleadoSeleccionado.id_empleado,
                    ct_NombreEmpleado.Text,
                    ct_DireccionEmpleado.Text,
                    telefonoEmpleado,
                    ct_CorreoEmpleado.Text,
                    ct_RolEmpleado.Text,
                    salarioEmpleado,
                    dp_FechaContrato.SelectedDate.Value,
                    horaEntrada,
                    horaSalida,
                    cb_EstadoEmpleado.Text,
                    dp_FechaFinContrato.SelectedDate.Value
                );

                //if (ValidarEmpleadosRepetidos(empleadoModificado))
                //{
                //    MostrarMensaje("No se permiten empleados repetidos");
                //    return;
                //}

                ConexionBD conexion = new ConexionBD();

                if (conexion.ModificarEmpleado(empleadoModificado))
                {
                    // Muestra si se modificó correctamente el empleado
                    MostrarMensaje("Empleado modificado correctamente");
                    VaciasCasillas();
                    ct_NombreEmpleado.Focus();
                    CargarEmpleados();
                }
                else
                {
                    // Muestra si no se modificó correctamente el empleado
                    MostrarMensaje("No se pudo modificar el empleado");
                }
            }
        }
    }
}
