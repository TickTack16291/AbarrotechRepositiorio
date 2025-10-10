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
        public RegistroProveedores()
        {
            InitializeComponent();
        }
        private void dg_Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void btn_Cancelar_Click(object sender, RoutedEventArgs e) {
            MenuOperaciones menuOperaciones = new MenuOperaciones();
            menuOperaciones.Show();
            this.Close();
        }

        private void btn_Guardar_Click(object sender, RoutedEventArgs e) {
        }

        private void btn_Modificar_Click(object sender, RoutedEventArgs e) {

        }
    }
}
