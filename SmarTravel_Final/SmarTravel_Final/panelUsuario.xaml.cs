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
    /// Lógica de interacción para panelUsuario.xaml
    /// </summary>
    public partial class panelUsuario : UserControl
    {
        string nameImagen = "";
        public static fichaPersonal ficha = null;

        public string nombreArchivo = "";
        public panelUsuario()
        {
            InitializeComponent();
        }
        private Boolean validarBusqueda()
        {
            string busqueda = busquedaUser.Text;

            if (busqueda == "")
            {
                validar mensajeValidacion = new validar();
                mensajeValidacion.show("Ingrese el atributo para realizar la busqueda.");
                return false;
            }
            else if (cbCargo.IsChecked == false && cbNombre.IsChecked == false && cbRut.IsChecked == false)
            {
                validar mensajeValidacion = new validar();
                mensajeValidacion.show("Seleccione uno de los tres parametros de realizar la busqueda.");
                return false;
            }
            return true;
        }

        

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Boolean validar = validarBusqueda();
            if (validar)
            {

                MySqlConnection con = conexionDB.ObtenerConexion();
                string comboSeleccionado = "";
                try
                {
                    if (cbCargo.IsChecked.Value)
                        comboSeleccionado = "CARGO";
                    else if (cbNombre.IsChecked.Value)
                        comboSeleccionado = "NOMBRE_COMPLETO";
                    else if (cbRut.IsChecked.Value)
                        comboSeleccionado = "RUT";

                    this.general.Visibility = Visibility.Hidden;
                    this.accionesUsuario.Visibility = Visibility.Hidden;
                    this.listadoTabla.Visibility = Visibility.Visible;
                    this.listadoUsuarios.Visibility = Visibility.Visible;

                    listadoUsuarios.ItemsSource = null;
                    string sql = "SELECT NOMBRE_COMPLETO,RUT,CARGO,FONO FROM PERSONA WHERE " + comboSeleccionado + " LIKE '%" + busquedaUser.Text + "%' ";

                    // Buneo
                    /*  MySqlCommand cmd = new MySqlCommand(sql, con);
                      MySqlDataAdapter adp = new MySqlDataAdapter(cmd);

                       

                  DataSet ds = new DataSet();
                  adp.Fill(ds, "cargarDatosBusqueda");
                  this.listadoUsuarios.DataContext = ds;      */
                    //
                    //  }

                    MySqlCommand cmdSel = new MySqlCommand(sql, con);
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);

                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        int largo = this.listadoUsuarios.Columns.Count;
                        for (int i = 0; i < largo; i++)
                        {
                            this.listadoUsuarios.Columns.RemoveAt(i);
                        }

                        //  this.listadoUsuarios.TableStyles[0].GridColumnStyles.RemoveAt(0);
                        //listadoUsuarios.ItemsSource = null;
                        this.listadoUsuarios.ItemsSource = dt.DefaultView;
                        this.listadoUsuarios.Columns[0].Width = 270;
                        this.listadoUsuarios.Columns[1].Width = 130;
                        this.listadoUsuarios.Columns[2].Width = 180;
                        this.listadoUsuarios.Columns[3].Width = 100;
                       // this.listadoUsuarios.Columns[4].Width = 100;
                        //this.listadoUsuarios.Columns[4].Width = 100;

                        /*DataGridTextColumn textColumn = new DataGridTextColumn();
                        textColumn.Header = "a";
                        
                        
                        //textColumn.Binding = new Binding("FirstName");
                        this.listadoUsuarios.Columns.Add(textColumn);

                        int numeroFilas = this.listadoUsuarios.Items.Count;

                        DataRowView drv = this.listadoUsuarios.CurrentCell.Item as DataRowView;

                      
                        //this.listadoUsuarios.Columns.Add(


                        // DataGridTextColumn textColumn = new DataGridTextColumn(); 
                        //textColumn.Header = "VER";
                        //textColumn.IsReadOnly = true;
                        // textColumn.Binding = new Binding("FirstName");
                        // textColumn.Width = 180;
                        //this.listadoUsuarios.Columns.Add(textColumn);
                        //this.listadoUsuarios.CurrentRow.Cells("DataGridViewTextBoxColumn8").Value = 20
                        //this.label12.Visibility = Visibility.Visible;
                        //this.label13.Visibility = Visibility.Visible;
                        */
                    }
                    else
                    {
                        this.listadoTabla.Visibility = Visibility.Hidden;
                        this.general.Visibility = Visibility.Visible;
                        this.accionesUsuario.Visibility = Visibility.Visible;
                        //this.busquedaUser.Text = "";
                        //this.cbCargo.IsChecked = false;
                        //this.cbNombre.IsChecked = false;
                        //this.cbRut.IsChecked = false;
                        validar mensaje = new validar();
                        mensaje.show("No hay resultado para lo que desea buscar");
                        listadoUsuarios.ItemsSource = null;
                        //this.listadoUsuarios.Visibility = Visibility.Hidden;
                        

                    }

                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } // FIn catch
            } // fin if validar

        }
        private void cbRut_Checked(object sender, RoutedEventArgs e)
        {
            if (cbRut.IsChecked.Value)
            {
                cbNombre.IsChecked = false;
                cbCargo.IsChecked = false;
            }
        }

        private void cbNombre_Checked(object sender, RoutedEventArgs e)
        {
            if (cbNombre.IsChecked.Value)
            {
                cbRut.IsChecked = false;
                cbCargo.IsChecked = false;
            }
        }

        private void cbCargo_Checked(object sender, RoutedEventArgs e)
        {
            if (cbCargo.IsChecked.Value)
            {
                cbRut.IsChecked = false;
                cbNombre.IsChecked = false;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            
            MySqlConnection con = conexionDB.ObtenerConexion();
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            //open.Filter = "Archivos jpg(*.jpg)|*.jpg";
            open.Filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|" +"All files (*.*)|*.*";
            open.Title = "Archivos Imagenes";
            string ruta = "";

            Nullable<bool> result = open.ShowDialog();
            if (result == true)
            {

                ruta = open.FileName;
                mensajeImagen.Visibility = Visibility.Visible;
                rutaImagen.Text = ruta;
                nameImagen = open.FileName;
                nombreArchivo = open.SafeFileName;
 

            }
        }
        private Boolean validarIngreso()
        {
            validar mensajeValidacion = new validar();
            if (nombreCompleto.Text == "")
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
            else if (rut.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el rut para el usuario.");
                return false;
            }
            else if (verificador.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el verificador del rut para el usuario.");
                return false;
            }
            else if (direccion.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese la direccion del residencia del usuario.");
                return false;
            }
            else if (comboRegion.Text == "")
            {
                mensajeValidacion.show("Por favor seleccione una region.");
                return false;
            }
            else if (clave.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese una clave.");
                return false;
            }
            else if (fono.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el numero de telefono");
                return false;
            }
            else if (comboSexo.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el sexo del usuario.");
                return false;
            }
            else if (comboCargo.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese el cargo del usuario.");
                return false;
            }
            else if (rutaImagen.Text == "")
            {
                mensajeValidacion.show("Por favor ingrese la imagen para el usuario.");
                return false;
            }
            return true;
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Boolean validar = validarIngreso();

            if (validar)
            {

                MySqlConnection con = conexionDB.ObtenerConexion();
                string nombre = nombreCompleto.Text;
                int edadUser = int.Parse(edad.Text);
                string rutUsuario = rut.Text;
                string rut_verificador = verificador.Text;
                string dire = direccion.Text;
                string ciudad = comboCiudad.Text;

                int telefono = int.Parse(fono.Text);
                string sexo = comboSexo.Text;
                string cargo = comboCargo.Text;
                string pass = clave.Text;

                string rutIngresado = rutUsuario + "-" + rut_verificador;
                try
                {

                    string sqlID_CIUDAD = "SELECT ID FROM CIUDAD WHERE NOMBRE = '" + ciudad + "'";
                    MySqlCommand cmd = new MySqlCommand(sqlID_CIUDAD, con);

                    MySqlDataReader dr = cmd.ExecuteReader();

                    int numeroCiudad = 0;
                    while (dr.Read())
                    {
                        numeroCiudad = dr.GetInt32(0);
                    }
                    con.Close();
                    con.Open();
                    /*System.IO.FileStream fs = new FileStream(rutaImagen.Text, FileMode.Open);
                    System.IO.BufferedStream bf = new BufferedStream(fs);
                    byte[] buffer = new byte[bf.Length];
                    bf.Read(buffer, 0, buffer.Length);

                    byte[] buffer_new = buffer;
                     */

                   
                    string path = System.IO.Directory.GetCurrentDirectory();
                    path = path.Substring(0, path.Length - 9);
                    path = path + "Images/fotoPerfil/";
                    string filePath = path + System.IO.Path.GetFileName(nameImagen);
                    
                  
                    System.IO.File.Copy(nameImagen, filePath, true);


                    //MySqlCommand cmdIns = new MySqlCommand(string.Format("INSERT INTO PERSONA (rut,NOMBRE_COMPLETO,edad,direccion,ciudad,fono,clave,imagen,sexo,cargo) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", rutIngresado, nombre, edadUser, dire, numeroCiudad, telefono, pass,filePath.ToString(), sexo, cargo), con);
                   
                    string insertString = "INSERT INTO PERSONA (rut,NOMBRE_COMPLETO,edad,direccion,ciudad,fono,clave,imagen,sexo,cargo) VALUES (?rut,?nombre,?edad,?direccion,?ciudad,?fono,?clave,?imagen,?sexo,?cargo)";
                    MySqlCommand insertCommand = new MySqlCommand(insertString, con);
                    insertCommand.Parameters.Add("?rut", rutIngresado);
                    insertCommand.Parameters.Add("?nombre", nombre);
                    insertCommand.Parameters.Add("?edad", edadUser);
                    insertCommand.Parameters.Add("?direccion",dire);
                    insertCommand.Parameters.Add("?ciudad", numeroCiudad);
                    insertCommand.Parameters.Add("?fono",telefono);
                    insertCommand.Parameters.Add("?clave", pass);
                    insertCommand.Parameters.Add("?imagen", nombreArchivo);
                    insertCommand.Parameters.Add("?sexo",sexo);
                    insertCommand.Parameters.Add("?cargo", cargo);


                    insertCommand.ExecuteNonQuery();
                    con.Close();


                    nombreCompleto.Text = "";
                    edad.Text = "";
                    rut.Text = "";
                    verificador.Text = "";
                    direccion.Text = "";
                    fono.Text = "";
                    clave.Text = "";
                    comboSexo.Text = "";
                    comboCiudad.Text = "";
                    comboCiudad.Items.Clear();
                    comboRegion.Text = "";
                    comboCargo.Text = "";
                    this.mensajeImagen.Visibility = Visibility.Hidden;
                    nuevoUsuario mensajeNuevo = new nuevoUsuario();
                    mensajeNuevo.show(nombre);
                }


                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private void rut_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void rut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void rut_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void fono_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void fono_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void verificador_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void verificador_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.K)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void comboRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboRegion_DropDownClosed(object sender, EventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)187, (byte)186, (byte)186));
            this.comboRegion.Foreground = colorFondo;
            comboCiudad.Text = "";
            comboCiudad.Items.Clear();

            int numeroRegion = 0;

            if (comboRegion.Text == "ARICA Y PARINACOTA")
            {
                numeroRegion = 15;
            }
            else if (comboRegion.Text == "LOS RIOS")
            {
                numeroRegion = 14;
            }
            else if (comboRegion.Text == "METROPOLITANA")
            {
                numeroRegion = 13;
            }
            else if (comboRegion.Text == "TARAPACA")
            {
                numeroRegion = 1;
            }
            else if (comboRegion.Text == "ANTOFAGASTA")
            {
                numeroRegion = 2;
            }
            else if (comboRegion.Text == "ATACAMA")
            {
                numeroRegion = 3;
            }
            else if (comboRegion.Text == "COQUIMBO")
            {
                numeroRegion = 4;
            }
            else if (comboRegion.Text == "VALPARAISO")
            {
                numeroRegion = 5;
            }
            else if (comboRegion.Text == "OHIGGINS")
            {
                numeroRegion = 6;
            }
            else if (comboRegion.Text == "MAULE")
            {
                numeroRegion = 7;
            }
            else if (comboRegion.Text == "BIO BIO")
            {
                numeroRegion = 8;
            }
            else if (comboRegion.Text == "ARAUCANIA")
            {
                numeroRegion = 9;
            }
            else if (comboRegion.Text == "LOS LAGOS")
            {
                numeroRegion = 10;
            }
            else if (comboRegion.Text == "AYSEN")
            {
                numeroRegion = 11;
            }
            else if (comboRegion.Text == "MAGALLANES")
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

                comboCiudad.Items.Clear();
                while (dr.Read())
                {
                    this.comboCiudad.Items.Add(dr.GetValue(0));
                }
                this.comboCiudad.SelectedIndex = 0;
                con.Close();
            }
            catch (Exception ex)
            {
                validar ventana = new validar();
                ventana.show(ex.Message);
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

        private void comboRegion_DropDownOpened(object sender, EventArgs e)
        {
            this.comboRegion.Foreground = Brushes.Black;
        }

        private void comboCiudad_DropDownOpened(object sender, EventArgs e)
        {
            this.comboCiudad.Foreground = Brushes.Black;
        }

        private void comboCiudad_DropDownClosed(object sender, EventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)187, (byte)186, (byte)186));
            this.comboCiudad.Foreground = colorFondo;
        }

        private void comboSexo_DropDownOpened(object sender, EventArgs e)
        {
            this.comboSexo.Foreground = Brushes.Black;
        }

        private void comboSexo_DropDownClosed(object sender, EventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)187, (byte)186, (byte)186));
            this.comboSexo.Foreground = colorFondo;
        }

        private void comboCargo_DropDownClosed(object sender, EventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)187, (byte)186, (byte)186));
            this.comboCargo.Foreground = colorFondo;
        }

        private void comboCargo_DropDownOpened(object sender, EventArgs e)
        {

            this.comboCargo.Foreground = Brushes.Black;
        }

        private void listadoUsuarios_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string rutTable = "";
            foreach (DataRowView dr in this.listadoUsuarios.SelectedItems)
            {

                rutTable = dr[1].ToString();


            }

            ficha = new fichaPersonal();
            ficha.llenarFicha(rutTable);
            ficha.ShowDialog();
        }

        private void ComboBoxItem_Loaded(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT NOMBRE FROM REGION ORDER BY ID ASC";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.comboRegion.Items.Add(dr.GetString(0));
            }
        }

        private void addUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.listadoTabla.Visibility = Visibility.Hidden;
            this.general.Visibility = Visibility.Visible;
            this.accionesUsuario.Visibility = Visibility.Visible;

            nombreCompleto.Text = "";
            edad.Text = "";
            rut.Text = "";
            verificador.Text = "";
            direccion.Text = "";
            fono.Text = "";
            clave.Text = "";
            comboSexo.Text = "";
            comboCiudad.Text = "";
            comboCiudad.Items.Clear();
            comboRegion.Text = "";
            comboCargo.Text = "";

            cbCargo.IsChecked = false;
            cbNombre.IsChecked = false;
            cbRut.IsChecked = false;

            busquedaUser.Text = "";

        }

        
    }
}
