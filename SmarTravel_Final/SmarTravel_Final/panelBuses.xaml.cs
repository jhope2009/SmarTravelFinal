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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data.Sql;
using System.Data;
using System.IO;

namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para panelBuses.xaml
    /// </summary>
    public partial class panelBuses : UserControl
    {
        string nameImagen = "";
        public panelBuses()
        {
            InitializeComponent();
        }
        public static Boolean validar1(params string[] values)
        {
            validar mensajeValidacion = new validar();
            for (int i = 0; i < values.Length; i += 2)
            {
                if (values[i] == "")
                {
                    mensajeValidacion.show("Falta llenar el siguiente campo:  " + values[i + 1]);
                    return false;
                }
            }
            return true;
        }
        private void btnagregarbus_Click(object sender, RoutedEventArgs e)
        {

            // Validar Campos antes de obtener contenido.

            if (validar1(txtModelo.Text, lbModelo.Text, comboMarca.Text, lbMarca.Text, cbxEstado.Text, lblEstado.Text, fechaPermiso.Text, label2.Text, año.Text, label5.Text, comboCiudad.Text, label6.Text) && validapatente1(txtPatente.Text))
            {
                
                string rbSeleccionado = "";

                if (rbSi.IsChecked == true)
                {
                    rbSeleccionado = "SI";
                }
                else
                {
                    rbSeleccionado = "NO";
                }


                try
                {
                    // Validar antes de esto, al comienzo....
                    //if (this.txtbRuta.Text.Equals("Ruta"))
                    //    MessageBox.Show("No ha seleccionado ruta");
                    //else if (patente.Equals("") || modelo.Equals(""))
                    //    MessageBox.Show("Falta llenar un campo");
                    //else
                    //{
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    MySqlCommand cmd;

                    string path = System.IO.Directory.GetCurrentDirectory();
                    path = path.Substring(0, path.Length - 9);
                    path = path + "Images/fotosBuses/";
                    string filePath = path + System.IO.Path.GetFileName(nameImagen);


                    System.IO.File.Copy(nameImagen, filePath, true);

                    string sqlID_CIUDAD = "SELECT ID FROM CIUDAD WHERE NOMBRE = '" + comboCiudad.Text + "'";
                    cmd = new MySqlCommand(sqlID_CIUDAD, con);

                    MySqlDataReader dr = cmd.ExecuteReader();

                    int numeroCiudad = 0;
                    while (dr.Read())
                    {
                        numeroCiudad = dr.GetInt32(0);
                    }
                    con.Close();

                    con.Open();
                    string insertString = "INSERT INTO BUS (patente,modelo,marca,year,vigencia_permiso,fecha_permiso,estado,imagen,ubicacion) VALUES (?patente,?modelo,?marca,?year,?vigencia,?fecha,?estado,?imagen,?ciudad)";
                    MySqlCommand insertCommand = new MySqlCommand(insertString, con);
                    insertCommand.Parameters.Add("?patente", txtPatente.Text);
                    insertCommand.Parameters.Add("?modelo", txtModelo.Text);
                    insertCommand.Parameters.Add("?marca", comboMarca.Text);
                    insertCommand.Parameters.Add("?year", año.Text);
                    insertCommand.Parameters.Add("?vigencia", rbSeleccionado);
                    insertCommand.Parameters.Add("?fecha", fechaPermiso.Text);
                    insertCommand.Parameters.Add("?estado", cbxEstado.Text);
                    insertCommand.Parameters.Add("?imagen", filePath.ToString());
                    insertCommand.Parameters.Add("?ciudad", numeroCiudad);


                    insertCommand.ExecuteNonQuery();

                    nuevoBus busNuevo = new nuevoBus();

                    busNuevo.Show();

                    this.txtModelo.Text = "";
                    this.comboMarca.SelectedIndex = -1;
                    this.txtPatente.Text = "";
                    this.cbxEstado.SelectedIndex = -1;

                    this.rbSi.IsChecked = false;
                    this.rbNo.IsChecked = false;
                    this.fechaPermiso.Text = "";
                    this.año.Text = "";
                    this.comboCiudad.SelectedIndex = -1;

                    string pathImagen = System.IO.Directory.GetCurrentDirectory();


                    pathImagen = pathImagen.Substring(0, pathImagen.Length - 9);

                    pathImagen = pathImagen + "Images/bus generico gris.jpg";

                    var uri = new Uri(pathImagen);
                    var bitmap = new BitmapImage(uri);
                    this.examinarBus.Source = bitmap;




                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
             
        }
             
        

        private void tabVerbuses_Selected(object sender, RoutedEventArgs e)
        {

            System.Data.DataTable dtBuses = new System.Data.DataTable("MyTable");
            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                MySqlCommand cmd;
                String query = "SELECT modelo as Modelo,marca as Marca,patente as Patente FROM BUS";

                cmd = new MySqlCommand(query, con);
                MySqlDataAdapter returnVal = new MySqlDataAdapter(query, con);
                returnVal.Fill(dtBuses);
                //this.gridBuses.Columns[0].Width = 160;
                this.gridBuses.DataContext = dtBuses.DefaultView;

                
                //this.gridBuses.Columns[1].Width = 160;
                //this.gridBuses.Columns[2].Width = 160;
                //this.gridBuses.Columns[3].Width = 100;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnExaminar_Click(object sender, RoutedEventArgs e)
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
                //this.txtbRuta.Visibility = Visibility.Visible;
                //this.txtbRuta.Text = ruta;
                nameImagen = open.FileName;

                var uri = new Uri(nameImagen);
                var bitmap = new BitmapImage(uri);
                examinarBus.Source = bitmap;
            }
        }
        private Boolean validapatente1(String patente) 
        {
            validar validapatente = new validar();
            if (patente.Length==8)
            {
                for (int i = 0; i < patente.Length; i++)
                {
                    if (i == 0 || i == 1)
                    {
                        if ((int)patente[i] < 65 || (int)patente[i] > 90)
                        {
                            Console.WriteLine("1");
                            validapatente.show("Formato de la patente incorrecto: EJ XX-XX-00 o XX-00-XX");
                            return false;
                        }
                    }
                    if (i == 2 || i == 5)
                    {
                        if (patente[i].Equals(' ')==false)
                        {
                            Console.WriteLine("2");
                            validapatente.show("Formato de la patente incorrecto: EJ XX-XX-00 o XX-00-XX");
                            return false;
                        }
                    }

                    if (i == 3 || i == 4)
                    {
                        if ((int)patente[i] < 48 || (int)patente[i] > 57 && (int)patente[i] < 65 || (int)patente[i] > 90)
                        {
                            Console.WriteLine("3");
                            validapatente.show("Formato de la patente incorrecto: EJ XX-XX-00 o XX-00-XX");
                            return false;
                        }
                    }
                    if (i == 6 || i == 7)
                    {
                        if ((int)patente[i] < 48 || (int)patente[i] > 57)
                        {
                            Console.WriteLine("4");
                            validapatente.show("Formato de la patente incorrecto ej: XX-XX-00 o XX-00-XX");
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                validapatente.show("Los caracteres de la patente deben ser 8");
                return false;
            }
        }

       
        private void gridBuses_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            /*
            foreach (var item in e.AddedCells)
            {
                var col = item.Column as DataGridColumn;
                var fc = col.GetCellContent(item.Item);

                if (fc is TextBlock)
                {
                    this.TXTPRUEBA.Text = (fc as TextBlock).Text;
                }

            }*/
        }


        private void gridBuses_CellEditEnding(object sender,
                                  DataGridCellEditEndingEventArgs e)
        {

            //this.TXTPRUEBA.Text = this.gridBuses.SelectedCells.ToString();
        }

        private void gridBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            string patenteBus = "";
            foreach (DataRowView dr in this.gridBuses.SelectedItems)
            {
               
                patenteBus = dr[2].ToString();
                

            }

            fichaBus ficha = new fichaBus();
            ficha.llenarFicha(patenteBus);
            ficha.Show();
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboMarca_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT NOMBRE FROM MARCA_BUS ORDER BY NOMBRE ASC";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.comboMarca.Items.Add(dr.GetString(0));
            }
        }

        private void año_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void comboCiudad_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT NOMBRE FROM CIUDAD ORDER BY NOMBRE ASC";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.comboCiudad.Items.Add(dr.GetString(0));
            }
        }

        private void fechaPermiso_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboMarca_DropDownOpened(object sender, EventArgs e)
        {
            this.comboMarca.Items.Clear();
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT NOMBRE FROM MARCA_BUS ORDER BY NOMBRE ASC";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.comboMarca.Items.Add(dr.GetString(0));
            }
        }
    
    }
}
