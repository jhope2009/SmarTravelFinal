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
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data.Sql;
using System.IO;

namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para fichaChofer.xaml
    /// </summary>
    public partial class fichaChofer : Window
    {
        public fichaChofer()
        {
            InitializeComponent();
        }

        public void llenarficha(string rut)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            try
            {
                string sql = "select p.nombre_completo , p.rut, c.numero_licencia, c.vencimiento_licencia, c.contrato, c.imagen_licencia ";
                sql += "from persona as p inner join chofer as c on p.rut = c.persona where p.rut ='"+rut+"'";
                Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    txtnombreChofer.Text = dr.GetValue(0).ToString();
                    txtrutchofer.Text = dr.GetValue(1).ToString();
                    txtNumerolicencia.Text = dr.GetValue(2).ToString();
                    dateLicencia.Text = dr.GetValue(3).ToString();
                    Console.WriteLine(dr.GetValue(5).ToString());
                    var uri = new Uri(dr.GetValue(5).ToString());
                    var bitmap = new BitmapImage(uri);
                    imglicencia.Source = bitmap;

                }
                con.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }

        private void image1_MouseUp(object sender, RoutedEventArgs e) 
        { 
        }


        private void datePicker2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            txtnombreChofer.IsReadOnly = false;
            txtnombreChofer.IsEnabled = true;
            txtNumerolicencia.IsReadOnly = false;
            txtNumerolicencia.IsEnabled = true;
            dateLicencia.IsEnabled = true;
        }

        private void btnCargarfoto_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Filter = "Archivos jpg(*.jpg)|*.jpg";
            open.Title = "Archivos Imagenes";
            string ruta = "";

            Nullable<bool> result = open.ShowDialog();
            if (result == true)
            {
                ruta = open.FileName;

            }

            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                path = path.Substring(0, path.Length - 9);
                path = path + "Images/fotoPerfil/";
                string filePath = path + System.IO.Path.GetFileName(ruta);



                string updateString = "UPDATE CHOFER SET imagen_licencia=?imagen WHERE persona=?rut";
                MySqlCommand updateCommand = new MySqlCommand(updateString, con);
                updateCommand.Parameters.Add("?imagen", filePath.ToString());
                updateCommand.Parameters.Add("?rut", txtrutchofer.Text);
                System.IO.File.Copy(ruta, filePath, true);
                updateCommand.ExecuteNonQuery();

                var uri = new Uri(filePath);
                var bitmap = new BitmapImage(uri);
                imglicencia.Source = bitmap;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void btnVerContrato_Click(object sender, RoutedEventArgs e)
        {
            string path = @"C:\Contratos\";
            Process.Start(path + txtrutchofer.Text+".pdf");
        }

        private void btnIngresarVacaciones_Click(object sender, RoutedEventArgs e)
        {
            validar vali=new validar();
            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                string updateString = "UPDATE vacaciones_trabajadores SET inicio=?inicio, termino=?termino WHERE  persona=?rut";
                MySqlCommand updateCommand = new MySqlCommand(updateString, con);
                updateCommand.Parameters.Add("?inicio", dateinicio.Text);
                updateCommand.Parameters.Add("?termino", dateTermino.Text);
                updateCommand.Parameters.Add("?rut", txtrutchofer.Text);
                updateCommand.ExecuteNonQuery();
                con.Close();
                vali.show("Se han ingresado con exito las vacaciones");
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (txtnombreChofer.IsReadOnly == false)
            {
                if (panelUsuario.validar1(txtnombreChofer.Text, lblNombre.Text, txtNumerolicencia.Text, lblNumerolicencia.Text))
                {
                    try
                    {
                        MySqlConnection con = conexionDB.ObtenerConexion();
                        string updateStringP = "UPDATE PERSONA SET nombre_completo=?nombre WHERE rut='"+txtrutchofer.Text+"'";
                        MySqlCommand updateCommand = new MySqlCommand(updateStringP, con);
                        updateCommand.Parameters.Add("?nombre", txtnombreChofer.Text);               
                        updateCommand.ExecuteNonQuery();
                        con.Close();
                        con.Open();
                        string updateStringC = "UPDATE CHOFER SET numero_licencia=?nlicencia WHERE persona='"+txtrutchofer.Text+"'";
                        MySqlCommand updateCommandC = new MySqlCommand(updateStringC, con);
                        updateCommand.Parameters.Add("?nlicencia", txtNumerolicencia.Text); 
                        updateCommand.ExecuteNonQuery();
                        con.Close();

                        nuevoUsuario update = new nuevoUsuario();
                        update.textBlock1.Text = "Se ha actualizado correctamente al chofer en el sistema.";
                        update.saludo.Visibility = Visibility.Hidden;
                        update.mensajeUpdate.Visibility = Visibility.Visible;
                        update.show("");
                        
                    }
                    catch (Exception ex)
                    {
                        validar ventana = new validar();
                        ventana.show(ex.Message);
                    }
         
                }

            }
            else
            {
                validar error = new validar();
                error.show("Seleccione el boton para editar el regitro");
            }
        }
        
    }
}
