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
using MySql.Data.MySqlClient;
using System.Data.Sql;
using System.IO;
namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para eliminarUsuario.xaml
    /// </summary>
    public partial class eliminarUsuario : Window
    {

        public eliminarUsuario()
        {
            InitializeComponent();
        }

        public void darFicha(fichaPersonal ficha)
        {
            ficha.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {


            // DELETE
            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();

                string updateString = "DELETE FROM PERSONA WHERE rut=?rut";
                MySqlCommand updateCommand = new MySqlCommand(updateString, con);
                updateCommand.Parameters.Add("?rut", this.user.Content);

                updateCommand.ExecuteNonQuery();

                validar ventana = new validar();
                ventana.show("El usuario se ha eliminado correctamente del sistema");
                con.Close();



                this.Close();
                //panelUsuario panel = new panelUsuario();
                panelUsuario.ficha.Close();
            }
            catch (Exception ex)
            {
                validar ventana = new validar();
                ventana.show(ex.Message);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
