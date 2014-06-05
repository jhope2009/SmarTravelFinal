using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para nuevoViaje.xaml
    /// </summary>
    public partial class nuevoViaje : Window
    {
        public nuevoViaje()
        {
            InitializeComponent();
        }
        public void show(string usuario)
        {


            this.ShowDialog();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
