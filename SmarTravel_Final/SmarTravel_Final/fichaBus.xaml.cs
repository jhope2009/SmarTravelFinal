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
    /// Lógica de interacción para fichaBus.xaml
    /// </summary>
    public partial class fichaBus : Window
    {
        public fichaBus()
        {
            InitializeComponent();
        }
        public void llenarFicha(string patente)
        {

            MySqlConnection con = conexionDB.ObtenerConexion();
            try
            {
                string sql = "SELECT MODELO,MARCA,PATENTE,YEAR,UBICACION,ESTADO,VIGENCIA_PERMISO,FECHA_PERMISO,IMAGEN FROM BUS WHERE PATENTE = '" + patente + "'";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dr = cmd.ExecuteReader();
                string ciudadConsulta = "";
                string regionBuscada = "";
                string ciudadBuscada = "";
                string vigencia = "";
                while (dr.Read())
                {

                    txtModelo.Text = dr.GetValue(0).ToString();
                    txtMarca.Text = dr.GetValue(1).ToString();
                    txtPatente.Text = dr.GetValue(2).ToString();
                    año.Text = dr.GetValue(3).ToString();
                    
                    ciudadConsulta = dr.GetValue(4).ToString();
                    comboEstado.Text = dr.GetValue(5).ToString();
                    vigencia = dr.GetValue(6).ToString();

                    if (vigencia == "SI")
                    {
                        rbSi.IsChecked = true;
                        rbNo.IsChecked = false;
                    }
                    else { 
                    rbSi.IsChecked = false;
                        rbNo.IsChecked = true;
                    }

                    fechaPermiso.Text = dr.GetValue(7).ToString();

                    string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
                    path = path.Substring(0, path.Length - 9);
                    path = path + "Images/fotosBuses/";
                    path = path + dr.GetValue(8);

                    var uri = new Uri(path);
                    perfilBus.Source = new BitmapImage(uri);
                    

                }
                con.Close();
                con.Open();

                
                // Obtener NOMBRE REGION Y NOMBRE CIUDAD
                string sqlCiudad = "SELECT NOMBRE,REGION FROM CIUDAD WHERE ID = '" + ciudadConsulta + " '";
                MySqlCommand cmd2 = new MySqlCommand(sqlCiudad, con);

                MySqlDataReader drCIUDAD = cmd2.ExecuteReader();

                while (drCIUDAD.Read())
                {
                    ciudadBuscada = drCIUDAD.GetValue(0).ToString();
                    regionBuscada = drCIUDAD.GetValue(1).ToString();

                }
                con.Close();

                // INDICAR INDICE A REGION.
                con.Open();
                string sqlRegion = "SELECT ID FROM REGION WHERE NOMBRE ='" + regionBuscada + "'";
                MySqlCommand cmd3 = new MySqlCommand(sqlRegion, con);

                MySqlDataReader drRegion = cmd3.ExecuteReader();
                int indice = 0;
                while (drRegion.Read())
                {
                    indice = drRegion.GetInt32(0);

                }

                region.SelectedIndex = indice - 1;
                con.Close();

                // DAR INDICE A CIUDAD
                con.Open();
                string sqlCiudadCombo = "SELECT NOMBRE FROM CIUDAD WHERE REGION ='" + regionBuscada + "'";

                MySqlCommand cmd4 = new MySqlCommand(sqlCiudadCombo, con);

                MySqlDataReader drCiudadCombo = cmd4.ExecuteReader();
                int numeroCiudad = 0;
                while (drCiudadCombo.Read())
                {
                    ciudad.Items.Add(drCiudadCombo.GetValue(0));

                    if (drCiudadCombo.GetValue(0).Equals(ciudadBuscada))
                    {
                        ciudad.SelectedIndex = numeroCiudad;
                    }
                    numeroCiudad++;



                }

                
                con.Close();


            }
            catch (Exception EX)
            {
                validar v = new validar();
                v.show(EX.Message);
            }


        }

        private void button1_Click(object sender, RoutedEventArgs e)
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
                path = path + "Images/fotosBuses/";
                string filePath = path + System.IO.Path.GetFileName(ruta);



                string updateString = "UPDATE BUS SET imagen=?imagen WHERE patente=?patente";
                MySqlCommand updateCommand = new MySqlCommand(updateString, con);
                updateCommand.Parameters.Add("?imagen", open.SafeFileName);
                updateCommand.Parameters.Add("?patente", txtPatente.Text);
                System.IO.File.Copy(ruta, filePath, true);
                updateCommand.ExecuteNonQuery();

                var uri = new Uri(filePath);
                var bitmap = new BitmapImage(uri);
                perfilBus.Source = bitmap;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
            
            txtModelo.IsReadOnly = false;
            txtMarca.IsReadOnly = false;
            comboEstado.IsEnabled = true;

            año.IsReadOnly = false;
            region.IsEnabled = true;
            ciudad.IsEnabled = true;

            rbSi.IsEnabled = true;
            rbNo.IsEnabled = true;

            
 

        }

        private void comboBox1_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT id,NOMBRE FROM REGION ORDER BY ID ASC";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader dr = cmd.ExecuteReader();
            int numeroRegion = 0;
            while (dr.Read())
            {
                region.Items.Add(dr.GetString(1));
                numeroRegion = dr.GetInt32(0);
            }


        }
        private Boolean validarUpdate()
        {
            /*
            validar mensajeValidacion = new validar();
            if (nombre.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el nombre del usuario.");
                return false;
            }
            else if (edad.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese la edad para el usuario.");
                return false;
            }
            else if (edad.Text == "0")
            {
                mensajeValidacion.show("Por favor ingrese una edad valida para el usuario.");
                return false;
            }

            else if (direccion.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese la direccion del residencia del usuario.");
                return false;
            }
            else if (region.Text == "")
            {
                mensajeValidacion.show("Por favor seleccione una region.");
                return false;
            }
            else if (ciudad.Text == "")
            {
                mensajeValidacion.show("Por favor seleccione una region.");
                return false;
            }

            else if (fono.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el numero de telefono");
                return false;
            }
            else if (clave.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese una clave.");
                return false;
            }
            else if (sexo.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el sexo del usuario.");
                return false;
            }
            else if (cargo.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el cargo del usuario.");
                return false;
            }
            */
            return true;
             
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            
            if (txtModelo.IsReadOnly == false)
            {
                if (validarUpdate())
                {
                    try
                    {
                        MySqlConnection con = conexionDB.ObtenerConexion();

                        string sqlID_CIUDAD = "SELECT ID FROM CIUDAD WHERE NOMBRE = '" + ciudad.Text + "'";
                        MySqlCommand cmd = new MySqlCommand(sqlID_CIUDAD, con);

                        MySqlDataReader dr = cmd.ExecuteReader();

                        int numeroCiudad = 0;
                        while (dr.Read())
                        {
                            numeroCiudad = dr.GetInt32(0);
                        }
                        con.Close();
                        // UPDATE

                        string vigencia = "";
                        if (rbSi.IsChecked == true){
                        vigencia="SI";
                        }
                        else
                         { 
                            vigencia="NO";
                         }

                        con.Open();
                        string updateString = "UPDATE BUS SET modelo=?modelo,marca=?marca,year=?year,fecha_permiso=?fecha_permiso,vigencia_permiso=?vigencia_permiso,estado=?estado,ubicacion=?ubicacion WHERE patente='" + txtPatente.Text + "'";
                        MySqlCommand updateCommand = new MySqlCommand(updateString, con);
                        updateCommand.Parameters.Add("?modelo", txtModelo.Text);
                        updateCommand.Parameters.Add("?marca", txtMarca.Text);
                        updateCommand.Parameters.Add("?year", año.Text);
                        updateCommand.Parameters.Add("?fecha_permiso", fechaPermiso.Text);
                        updateCommand.Parameters.Add("?vigencia_permiso", vigencia);
                        updateCommand.Parameters.Add("?estado", comboEstado.Text);
                        updateCommand.Parameters.Add("?ubicacion", numeroCiudad);
                        updateCommand.ExecuteNonQuery();
                        con.Close();

                        nuevoBus busNuevo = new nuevoBus();
                        busNuevo.textBlock1.Text = "Se ha actualizado correctamente el bus en el sistema";
                        busNuevo.saludo.Text = "Gracias por actualizar nuestros registros";
                        busNuevo.show("");
                        

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

        private void edad_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void edad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void fono_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void region_DropDownClosed(object sender, EventArgs e)
        {
            ciudad.Text = "";
            ciudad.Items.Clear();

            int numeroRegion = 0;

            if (region.Text == "ARICA Y PARINACOTA")
            {
                numeroRegion = 15;
            }
            else if (region.Text == "LOS RIOS")
            {
                numeroRegion = 14;
            }
            else if (region.Text == "METROPOLITANA")
            {
                numeroRegion = 13;
            }
            else if (region.Text == "TARAPACA")
            {
                numeroRegion = 1;
            }
            else if (region.Text == "ANTOFAGASTA")
            {
                numeroRegion = 2;
            }
            else if (region.Text == "ATACAMA")
            {
                numeroRegion = 3;
            }
            else if (region.Text == "COQUIMBO")
            {
                numeroRegion = 4;
            }
            else if (region.Text == "VALPARAISO")
            {
                numeroRegion = 5;
            }
            else if (region.Text == "OHIGGINS")
            {
                numeroRegion = 6;
            }
            else if (region.Text == "MAULE")
            {
                numeroRegion = 7;
            }
            else if (region.Text == "BIO BIO")
            {
                numeroRegion = 8;
            }
            else if (region.Text == "ARAUCANIA")
            {
                numeroRegion = 9;
            }
            else if (region.Text == "LOS LAGOS")
            {
                numeroRegion = 10;
            }
            else if (region.Text == "AYSEN")
            {
                numeroRegion = 11;
            }
            else if (region.Text == "MAGALLANES")
            {
                numeroRegion = 12;
            }

            MySqlDataReader dr;
            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                MySqlCommand cmd;

                string sql = "SELECT NOMBRE FROM CIUDAD WHERE NUMERO = " + numeroRegion + " ORDER BY NOMBRE ASC";
                cmd = new MySqlCommand(sql, con);

                dr = cmd.ExecuteReader();

                ciudad.Items.Clear();
                while (dr.Read())
                {
                    ciudad.Items.Add(dr.GetValue(0));
                }
                this.ciudad.SelectedIndex = 0;
                con.Close();
            }
            catch (Exception ex)
            {
                validar ventana = new validar();
                ventana.show(ex.Message);
            }
        }

        private void image1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        public void image1_MouseUp(object sender, MouseButtonEventArgs e)
        {

            //eliminarUsuario delete = new eliminarUsuario();
            //delete.delete.Text = "¿ Desea eliminar al usuario del sistema? ";
            //delete.Show();
            //delete.user.Content = rutUser.Text;
            // delete.ObjetoVentana(this.fichaPersonal);
        }
    }
}
