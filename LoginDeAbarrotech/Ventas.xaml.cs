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
    /// Lógica de interacción para Ventas.xaml
    /// </summary>
    public partial class Ventas : Window
    {
        public Ventas()
        {
            InitializeComponent();
            CargarProductosDisponibles();
        }
        public void CargarProductosDisponibles()
        {
            ConexionBD conexion = new ConexionBD();
            var producto = conexion.ObtenerProductosDisponibles();
            dg_ProductosDisponibles.ItemsSource = producto;
        }
        private void btn_cancelar_Click_1(object sender, RoutedEventArgs e)
        {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Hide();
        }
    }
}
