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
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Collections.ObjectModel;
using SmarTravel_Final.Controller;

namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para panelRecorrido.xaml
    /// </summary>
    public partial class panelRecorrido : UserControl
    {
        private Dictionary<string, int> ciudades = new Dictionary<string, int>();
        List<Ciudad> ciudades2 = new List<Ciudad>();
        //private Dictionary<int, int> recorridos;
        List<Recorrido> recorridos;
        List<string> ciudadOrigen = new List<string>();
        List<string> ciudadesDistintas = new List<string>();
        List<string> ciudadDestino = new List<string>();
        List<int> id_recorrido = new List<int>();
        List<int> id_recorrido_seleccion = new List<int>();
        List<string> paradas = new List<string>();
        //Di<string> ciudadDestino = new List<string>();

        List<string> intermedios = new List<string>();
        public panelRecorrido()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ciudades2 = CiudadFacade.buscarTodos();
            foreach (Ciudad c in this.ciudades2)
            {
                this.listCiudad.Items.Add((string)c.nombre);
                this.ciudades.Add(c.nombre, c.id);
            }

            MySqlConnection con = conexionDB.ObtenerConexion();
            try
            {

                string sql = "SELECT NOMBRE FROM REGION ORDER BY ID";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader data = cmd.ExecuteReader();

                while (data.Read())
                {
                    ComboBoxItem r = new ComboBoxItem();
                    r.Content = data.GetString(0);
                    this.comboRegion.Items.Add(r);
                }
                data.Close();

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

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listCiudad_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.listCiudad.SelectedItem != null)
            {
                this.listNuevoRecorrido.Items.Add(this.listCiudad.SelectedItem.ToString());
                this.listCiudad.Items.Remove(this.listCiudad.SelectedItem);
            }
        }

        private void listNuevoRecorrido_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.listNuevoRecorrido.SelectedItem != null)
            {
                this.listCiudad.Items.Add(this.listNuevoRecorrido.SelectedItem.ToString());
                this.listNuevoRecorrido.Items.Remove(this.listNuevoRecorrido.SelectedItem);
            }
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            this.listNuevoRecorrido.Items.Clear();
            this.listCiudad.Items.Clear();

            foreach (KeyValuePair<string, int> city in this.ciudades)
            {
                this.listCiudad.Items.Add(city.Key);
            }
        }
        
        private void listRecorridos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.listParadas.Items.Clear();

            try
            {
                Recorrido reco = this.recorridos[this.listRecorridos.SelectedIndex];
                Parada parada = reco.parada;

                while (parada.id != -1)
                {
                    this.listParadas.Items.Add(parada.ciudad.nombre);
                    parada = parada.siguiente;
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private int obtenerNumeroCiudadByRegion(string region)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT id FROM region WHERE nombre = '" + region + "'";
            int buscado = 0;
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                buscado = dr.GetInt32(0);
            }
            con.Close();
            return buscado;
        }


        private void crearCiudad_Click(object sender, RoutedEventArgs e)
        {
            if (this.textCiudad.Text != "")
            {
                if (this.ciudades.ContainsKey(this.textCiudad.Text) == false)
                {
                    if (this.comboRegion.SelectedIndex > -1)
                    {

                        string nombre = (string)this.textCiudad.Text;
                        int numero = (int)this.comboRegion.SelectedIndex;

                        // OBTENER NUMERO REGION BY REGION
                        try
                        {
                            //btenerNumeroCiudadByRegion(comboRegion.Text)
                            MySqlConnection con = conexionDB.ObtenerConexion();
                            string insertString = "INSERT INTO CIUDAD (NOMBRE,REGION,NUMERO) VALUES (?nombre,?region,?numero)";

                            MySqlCommand cmd = new MySqlCommand(insertString, con);
                            cmd.Parameters.Add("?nombre", nombre);
                            cmd.Parameters.Add("?region", comboRegion.Text);
                            cmd.Parameters.Add("?numero", obtenerNumeroCiudadByRegion(comboRegion.Text));

                            cmd.ExecuteNonQuery();
                            con.Close();

                            textCiudad.Text = "";
                            comboRegion.SelectedIndex = -1;
                            alerta alert = new alerta();
                            alert.show("Ciudad Ingresada Correctamente");

                            try
                            {
                                con.Open();
                                this.ciudades.Clear();
                                this.listCiudad.Items.Clear();
                                string sql = "SELECT C.ID, C.NOMBRE FROM CIUDAD AS C INNER JOIN REGION AS R ON (C.NUMERO=R.ID) ORDER BY R.ID";
                                cmd = new MySqlCommand(sql, con);
                                MySqlDataReader dr = cmd.ExecuteReader();

                                while (dr.Read())
                                {
                                    ciudades.Add(dr.GetString(1), dr.GetInt32(0));
                                    this.listCiudad.Items.Add(dr.GetString(1));
                                }
                                dr.Close();
                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                validar alerta = new validar();
                                alerta.show("LISTAR CIUDAD: " + ex.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            validar alert = new validar();
                            alert.show("INGRESA CIUDAD: " + ex.ToString());
                        }
                        finally
                        {
                            //con.Close();
                        }
                    }
                    else
                    {
                        validar alert = new validar();
                        alert.show("Debe seleccionar una Region");
                        this.comboRegion.Focus();
                    }
                }
                else
                {
                    validar alert = new validar();
                    alert.show("La ciudad ingresada ya existe");
                    this.textCiudad.Focus();
                }
            }
            else
            {
                validar alert = new validar();
                alert.show("Debe ingresar un nombre para la Parada");
                this.textCiudad.Focus();

            }
        }
        
        private void validarHora(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            string pattern = "\\d{1,2}:\\d{2}";

            if (box != null)
            {
                if (!Regex.IsMatch(box.Text, pattern, RegexOptions.CultureInvariant))
                {
                    MessageBox.Show("Not a valid time format ('hh:mm AM|PM').");
                    box.Select(0, box.Text.Length);
                }
            }
        }
        private Boolean validarIngresoViajeDiario()
        {
            validar mensajeValidacion = new validar();
            if (comboDestino.SelectedIndex == -1)
            {
                mensajeValidacion.show("Seleccione el destino del viaje.");
                return false;
            }
            else if (identificador.Text == "")
            {

                mensajeValidacion.show("Ingrese el identificador del viaje.");
                return false;
            }
            return true;


        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            horarios.Children.Clear();
            horarios.ColumnDefinitions.Clear();
            horarios.RowDefinitions.Clear();
            paradas.Clear();
            TextBox tHorarios;

            if (validarIngresoViajeDiario())
            {
                this.datosViaje.Visibility = Visibility.Visible;

                try
                {
                    // ENCABEZADO 3 COLUMNAS EN LA PRIMERA FILA

                    this.horarios.ColumnDefinitions.Add(new ColumnDefinition());
                    this.horarios.ColumnDefinitions.Add(new ColumnDefinition());
                    this.horarios.ColumnDefinitions.Add(new ColumnDefinition());
                    this.horarios.RowDefinitions.Add(new RowDefinition());


                    Label llegadaHeader = new Label();
                    llegadaHeader.Content = "LLEGADA";
                    llegadaHeader.Style = Resources["HeaderTabla"] as Style;

                    llegadaHeader.SetValue(Grid.ColumnProperty, 0);
                    llegadaHeader.SetValue(Grid.RowProperty, 0);
                    this.horarios.Children.Add(llegadaHeader);

                    Label salidadHeader = new Label();
                    salidadHeader.Content = "SALIDA";
                    salidadHeader.Style = Resources["HeaderTabla"] as Style;

                    salidadHeader.SetValue(Grid.ColumnProperty, 1);
                    salidadHeader.SetValue(Grid.RowProperty, 0);
                    this.horarios.Children.Add(salidadHeader);

                    Label intermediosHeader = new Label();
                    intermediosHeader.Content = "INTERMEDIOS";
                    intermediosHeader.Style = Resources["HeaderTabla"] as Style;
                    intermediosHeader.Width = 200;
                    intermediosHeader.SetValue(Grid.ColumnProperty, 2);
                    intermediosHeader.SetValue(Grid.RowProperty, 0);
                    this.horarios.Children.Add(intermediosHeader);


                    MySqlConnection con = conexionDB.ObtenerConexion();
                    string sql = "SELECT CIUDAD FROM PARADA WHERE RECORRIDO = '" + lbl_id_recorrido.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        paradas.Add(dr.GetString(0));


                    }
                    con.Close();

                    int largo = paradas.Count;
                    //MessageBox.Show("Largo parada"+largo);
                    for (int z = 0; z < largo; z++)
                    {

                        //this.horarios.ColumnDefinitions.Add(new ColumnDefinition());
                        this.horarios.RowDefinitions.Add(new RowDefinition());
                        //this.horarios.ColumnDefinitions[z].Width = new System.Windows.GridLength(100);
                        //this.horarios.RowDefinitions[z].Height = new System.Windows.GridLength(50);
                    }
                    int contador = 0;
                    for (int row = 1; row < this.horarios.RowDefinitions.Count; row++)
                    {
                        for (int col = 0; col < this.horarios.ColumnDefinitions.Count; col++)
                        {
                            if ((col == 0 && row == 1) || (col == 1 && row == horarios.RowDefinitions.Count - 1))
                            {
                                tHorarios = new TextBox();
                                tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                                tHorarios.Text = "-";
                                tHorarios.SetValue(Grid.ColumnProperty, col);
                                tHorarios.SetValue(Grid.RowProperty, row);
                                this.horarios.Children.Add(tHorarios);
                            }
                            else if (col == 2 && row != 0)
                            {
                                if (contador < paradas.Count)
                                {
                                    string ciudadBuscada = "";
                                    con.Open();
                                    sql = "SELECT NOMBRE FROM CIUDAD WHERE ID = '" + paradas[contador] + "'";
                                    cmd = new MySqlCommand(sql, con);
                                    dr = cmd.ExecuteReader();

                                    while (dr.Read())
                                    {
                                        ciudadBuscada = dr.GetString(0);
                                    }
                                    tHorarios = new TextBox();
                                    tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                                    tHorarios.Text = ciudadBuscada;
                                    tHorarios.SetValue(Grid.ColumnProperty, col);
                                    tHorarios.SetValue(Grid.RowProperty, row);
                                    this.horarios.Children.Add(tHorarios);

                                    con.Close();
                                }
                            }
                            else
                            {
                                tHorarios = new TextBox();
                                tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                                tHorarios.Text = "00:00";
                                tHorarios.MaxLength = 5;
                                tHorarios.IsReadOnly = false;
                                tHorarios.SetValue(Grid.ColumnProperty, col);
                                tHorarios.SetValue(Grid.RowProperty, row);
                                this.horarios.Children.Add(tHorarios);
                            }
                        }
                        contador++;
                    }
                }

                catch (Exception ex)
                {
                    validar validarError = new validar();
                    validarError.show(ex.Message);
                }
            }
        }

        private void comboBox5_Loaded(object sender, RoutedEventArgs e)
        {
            this.comboBus.Items.Clear();
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT PATENTE FROM BUS ORDER BY PATENTE ASC";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.comboBus.Items.Add(dr.GetString(0));
            }
        }



        private void comboAuxiliar_Loaded(object sender, RoutedEventArgs e)
        {
            this.comboAuxiliar.Items.Clear();
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT NOMBRE_COMPLETO FROM PERSONA WHERE CARGO ='AUXILIAR' ORDER BY NOMBRE_COMPLETO ASC";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.comboAuxiliar.Items.Add(dr.GetString(0));
            }
        }

        private void comboChofer_Loaded(object sender, RoutedEventArgs e)
        {
            this.comboChofer.Items.Clear();
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT NOMBRE_COMPLETO FROM PERSONA WHERE CARGO ='CHOFER' ORDER BY NOMBRE_COMPLETO ASC";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                this.comboChofer.Items.Add(dr.GetString(0));

            }
        }


        private void tabViajesDiarios_Selected(object sender, RoutedEventArgs e)
        {
            this.datosViaje.Visibility = Visibility.Hidden;
            this.comboDestino.Items.Clear();
            this.identificador.Text = "";
        }
        private void tabEditarViajes_Selected(object sender, RoutedEventArgs e)
        {
            this.listadoRecorridos.Visibility = Visibility.Hidden;
            this.cDestinoEditar.Items.Clear();
        }
        private void tabRecorridos_Selected(object sender, RoutedEventArgs e)
        {
            this.comboOrigenReco.SelectedIndex = -1;
            this.comboDestinoReco.Items.Clear();

            this.listParadas.Items.Clear();
            this.listRecorridos.Items.Clear();
        }
        private void tabNuevoRecorrido_Selected(object sender, RoutedEventArgs e)
        {
            this.listNuevoRecorrido.Items.Clear();
            this.listCiudad.Items.Clear();

            foreach (KeyValuePair<string, int> city in this.ciudades)
            {
                this.listCiudad.Items.Add(city.Key);
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {

            listadoRecorridos.RowDefinitions.Clear();
            listadoRecorridos.ColumnDefinitions.Clear();
            List<string> listIdentificador = new List<string>();
            List<string> listRecorrido = new List<string>();
            List<string> listViajes = new List<string>();

            List<string> listHorarioSalida = new List<string>();
            List<string> listHorarioLlegada = new List<string>();

            this.listadoRecorridos.Visibility = Visibility.Visible;


            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                int nombreOrigen = obtenerIdCiudad(this.cOrigenEditar.Text);
                int nombreDestino = obtenerIdCiudad(this.cDestinoEditar.Text);
                string sql = "SELECT V.ID,V.IDENTIFICADOR, T.RECORRIDO FROM TRAYECTO AS T INNER JOIN VIAJES AS V ON T.RECORRIDO = V.RECORRIDO WHERE ORIGEN = " + nombreOrigen + " AND DESTINO = " + nombreDestino;
                MySqlCommand cmd = new MySqlCommand(sql, con);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listViajes.Add(dr.GetString(0));
                    listIdentificador.Add(dr.GetString(1));
                    listRecorrido.Add(dr.GetString(2));

                }
                con.Close();

                // Obtener HORARIO DE SALIDA Y LLEGADA
                for (int i = 0; i < listViajes.Count; i++)
                {
                    con.Open();
                    sql = "SELECT SALIDA FROM HORARIOS WHERE LLEGADA = '-' AND VIAJE =" + listViajes[i];
                    cmd = new MySqlCommand(sql, con);

                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        listHorarioSalida.Add(dr.GetString(0));

                    }
                    con.Close();

                    con.Open();
                    string sqlHoraLlegada = "SELECT LLEGADA FROM HORARIOS WHERE SALIDA = '-' AND VIAJE =" + listViajes[i];

                    MySqlCommand cmd2 = new MySqlCommand(sqlHoraLlegada, con);

                    MySqlDataReader dr2 = cmd2.ExecuteReader();

                    while (dr2.Read())
                    {

                        listHorarioLlegada.Add(dr2.GetString(0));


                    }
                    con.Close();

                }

                darFormatoListadoRecorridos();

                // CREAR FILAS SEGUN RESULTADO
                int largo = listViajes.Count;
                for (int z = 0; z < largo; z++)
                {


                    this.listadoRecorridos.RowDefinitions.Add(new RowDefinition());

                }


                int contador = 0;
                TextBox tHorarios;
                for (int row = 1; row < this.listadoRecorridos.RowDefinitions.Count; row++)
                {
                    for (int col = 0; col < this.listadoRecorridos.ColumnDefinitions.Count; col++)
                    {

                        if (col == 0)
                        {
                            tHorarios = new TextBox();
                            tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                            tHorarios.Text = obtenerNombreCiudad(nombreOrigen);
                            tHorarios.SetValue(Grid.ColumnProperty, col);
                            tHorarios.SetValue(Grid.RowProperty, row);
                            tHorarios.IsReadOnly = true;
                            this.listadoRecorridos.Children.Add(tHorarios);

                        }
                        if (col == 1)
                        {
                            tHorarios = new TextBox();
                            tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                            tHorarios.Text = obtenerNombreCiudad(nombreDestino);
                            tHorarios.SetValue(Grid.ColumnProperty, col);
                            tHorarios.SetValue(Grid.RowProperty, row);
                            tHorarios.IsReadOnly = true;
                            this.listadoRecorridos.Children.Add(tHorarios);

                        }
                        if (col == 2)
                        {
                            tHorarios = new TextBox();
                            tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                            tHorarios.Text = listIdentificador[contador];
                            tHorarios.SetValue(Grid.ColumnProperty, col);
                            tHorarios.SetValue(Grid.RowProperty, row);
                            tHorarios.IsReadOnly = true;
                            this.listadoRecorridos.Children.Add(tHorarios);

                        }
                        if (col == 3)
                        {
                            tHorarios = new TextBox();
                            tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                            tHorarios.Text = listHorarioSalida[contador] + " - " + listHorarioLlegada[contador];
                            tHorarios.SetValue(Grid.ColumnProperty, col);
                            tHorarios.SetValue(Grid.RowProperty, row);
                            tHorarios.IsReadOnly = true;
                            this.listadoRecorridos.Children.Add(tHorarios);

                        }
                        if (col == 4)
                        {
                            BitmapImage btm = new BitmapImage(new Uri("/SmarTravel_Final;component/Images/busquedaIcon.png", UriKind.Relative));
                            Image img = new Image();
                            img.Source = btm;
                            img.Stretch = Stretch.Fill;
                            img.Width = 30;
                            img.Height = 30;
                            Button b = new Button();
                            b.Click += new RoutedEventHandler(b_Click);

                            //b.Content = "VER"
                            b.Content = img;

                            b.Tag = Convert.ToString(listViajes[contador]);
                            b.SetValue(Grid.ColumnProperty, col);
                            b.SetValue(Grid.RowProperty, row);


                            this.listadoRecorridos.Children.Add(b);

                        }
                    }
                    contador++;

                }

            }
            catch (Exception ex)
            {
                validar v = new validar();
                v.show(ex.Message);
            }





        }
        public void b_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var fila = button.Tag;


            editarViajeDiario edit = new editarViajeDiario();
            edit.Show();

            edit.getIdViaje(Convert.ToInt32(fila.ToString()));
            edit.crearTabla();


        }
        private void darFormatoListadoRecorridos()
        {

            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());
            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());
            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());
            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());
            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());
            this.listadoRecorridos.RowDefinitions.Add(new RowDefinition());


            Label origenHeader = new Label();
            origenHeader.Content = "ORIGEN";
            origenHeader.Style = Resources["HeaderTabla"] as Style;

            origenHeader.SetValue(Grid.ColumnProperty, 0);
            origenHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(origenHeader);

            Label destinoHeader = new Label();
            destinoHeader.Content = "DESTINO";
            destinoHeader.Style = Resources["HeaderTabla"] as Style;

            destinoHeader.SetValue(Grid.ColumnProperty, 1);
            destinoHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(destinoHeader);

            Label identificadorHeader = new Label();
            identificadorHeader.Content = "IDENTIFICADOR";
            identificadorHeader.Style = Resources["HeaderTabla"] as Style;
            identificadorHeader.Width = 200;
            identificadorHeader.SetValue(Grid.ColumnProperty, 2);
            identificadorHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(identificadorHeader);

            Label horarioHeader = new Label();
            horarioHeader.Content = "HORARIO";
            horarioHeader.Style = Resources["HeaderTabla"] as Style;
            horarioHeader.Width = 200;
            horarioHeader.SetValue(Grid.ColumnProperty, 3);
            horarioHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(horarioHeader);

            Label verHeader = new Label();
            verHeader.Content = "";
            verHeader.Style = Resources["HeaderTabla"] as Style;
            verHeader.Width = 100;
            verHeader.SetValue(Grid.ColumnProperty, 4);
            verHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(verHeader);
            int precio = this.listadoRecorridos.Children.Count;
        }

        private void comboOrigen_Loaded(object sender, RoutedEventArgs e)
        {


            this.comboOrigen.Items.Clear();
            ciudadOrigen.Clear();
            ciudadesDistintas.Clear();
            ciudadDestino.Clear();
            id_recorrido.Clear();
            id_recorrido_seleccion.Clear();
            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                string sql = "SELECT MAX(RECORRIDO) FROM PARADA";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dr = cmd.ExecuteReader();

                int numeroRecorridos = 0;
                if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {
                        numeroRecorridos = dr.GetInt32(0);
                    }

                    con.Close();

                    int i;

                    // RECORRO LOS RECORRIDOS GENERALES.
                    for (i = 0; i <= numeroRecorridos; i++)
                    {
                        con.Open();
                        string sqlRecorridos = "SELECT NOMBRE FROM PARADA INNER JOIN CIUDAD ON PARADA.CIUDAD = CIUDAD.ID WHERE RECORRIDO = '" + i + "' LIMIT 1";
                        cmd = new MySqlCommand(sqlRecorridos, con);

                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            // GUARDO TODOS LOS ORIGENES
                            ciudadOrigen.Add(dr.GetString(0));
                            // ID RECORRIDO
                            id_recorrido.Add(i);


                        }
                        con.Close();
                    }

                    con.Open();
                    string sqlciudadDestino = "SELECT NOMBRE FROM PARADA INNER JOIN CIUDAD ON PARADA.CIUDAD = CIUDAD.ID WHERE SIGUIENTE = -1";
                    cmd = new MySqlCommand(sqlciudadDestino, con);

                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ciudadDestino.Add(dr.GetString(0));
                    }
                    con.Close();

                    ciudadesDistintas = ciudadOrigen.Distinct().ToList();

                    for (int j = 0; j < ciudadesDistintas.Count; j++)
                    {
                        comboOrigen.Items.Add(ciudadesDistintas[j]);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void comboOrigen_DropDownClosed(object sender, EventArgs e)
        {
            id_recorrido_seleccion.Clear();
            comboDestino.Items.Clear();
            // OBTENER ID CIUDAD ELEGIDA.

            string ciudadOrigenElegida = this.comboOrigen.Text;

            try
            {
                // RECORRER LISTADO DE ORIGENES
                for (int i = 0; i < ciudadOrigen.Count; i++)
                {
                    //int cityOrigen= Convert.ToInt32(ciudadOrigen[i]);

                    if (ciudadOrigen[i].Equals(ciudadOrigenElegida))
                    {
                        id_recorrido_seleccion.Add(id_recorrido[i]);

                        this.comboDestino.Items.Add(ciudadDestino[i]);
                    }
                }
            }

            catch (Exception ex)
            {
                validar errorCiudad = new validar();
                errorCiudad.show(ex.Message);
            }
        }

        private void comboDestino_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                int seleccion = this.comboDestino.SelectedIndex;
                this.lbl_id_recorrido.Text = Convert.ToString(this.id_recorrido_seleccion[seleccion]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private Boolean validarInsertViajeDiario()
        {

            validar mensajeValidacion = new validar();
            string fechaActual = DateTime.Today.ToString("dd-MM-yyyy");

            if (comboChofer.SelectedIndex == -1)
            {
                mensajeValidacion.show("Seleccione el chofer para el viaje.");
                return false;
            }
            else if (comboAuxiliar.SelectedIndex == -1)
            {

                mensajeValidacion.show("Seleccione el auxiliar para el viaje.");
                return false;
            }
            else if (comboBus.SelectedIndex == -1)
            {

                mensajeValidacion.show("Seleccione el bus para el viaje.");
                return false;
            }
            else if (fechaDesde.Text == "")
            {

                mensajeValidacion.show("Seleccione la fecha inicial del viaje.");
                return false;
            }
            else if (fechaHasta.Text == "")
            {

                mensajeValidacion.show("Seleccione la fecha final para el viaje.");
                return false;
            }

            else if (DateTime.Parse(fechaActual).CompareTo(DateTime.Parse(this.fechaDesde.Text)) > 0)
            {
                mensajeValidacion.show("La fecha inicial es menor que la fecha actual.");
                return false;
            }
            else if (DateTime.Parse(this.fechaDesde.Text).CompareTo(DateTime.Parse(this.fechaHasta.Text)) > 0)
            {
                mensajeValidacion.show("La fecha inicial es menor que la fecha final.");
                return false;
            }
            foreach (UIElement ui in this.horarios.Children)
            {
                int row = System.Windows.Controls.Grid.GetRow(ui);
                int col = System.Windows.Controls.Grid.GetColumn(ui);

                if ((row == 0) && (col == 0 || col == 1 || col == 2))
                {
                    Label label = (Label)ui;
                }
                else
                {
                    TextBox txt = (TextBox)ui;
                    string textoCelda = txt.Text;
                    if (!textoCelda.Equals(""))
                    {
                        if (!textoCelda.Substring(0, 1).Equals("-") && (row > 0 && col < 2))
                        {
                            try
                            {
                                int horas = Convert.ToInt32(textoCelda.Substring(0, 2));
                                //int horas = Int32.Parse(textoCelda.Substring(0, 2));
                                int minutos = Convert.ToInt32(textoCelda.Substring(3, 2));
                                if (!textoCelda.Substring(2, 1).Equals(":"))
                                {
                                    mensajeValidacion.show("El horario debe tener el formato hh:mm");
                                    return false;
                                }

                                if (horas > 24)
                                {
                                    mensajeValidacion.show("La hora no debe ser superior a 24 horas");
                                    return false;
                                }

                                if (minutos > 60)
                                {
                                    mensajeValidacion.show("La minutos no deben ser superior a 60 minutos");
                                    return false;
                                }
                            }
                            catch
                            {
                                mensajeValidacion.show("Un horario se encuentra mal ingresado, no ingrese letras en el horario.");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        mensajeValidacion.show("Existe al menos un horario que no ha sido ingresado.");
                        return false;
                    }

                }
            }

            return true;
        }

        private int obtenerIdCiudad(string nombreCiudad)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT ID FROM CIUDAD WHERE NOMBRE ='" + nombreCiudad + "'";
            int ciudadBuscada = 0;
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ciudadBuscada = (dr.GetInt32(0));

            }
            con.Close();
            return ciudadBuscada;
        }

        private string obtenerNombreCiudad(int id)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT NOMBRE FROM CIUDAD WHERE ID ='" + id + "'";
            string nombreCiudad = "";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                nombreCiudad = (dr.GetString(0));

            }
            con.Close();
            return nombreCiudad;
        }
        private string obtenerRutPersona(string nombreCompleto, string cargo)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT RUT FROM PERSONA WHERE NOMBRE_COMPLETO = '" + nombreCompleto + "' AND CARGO = '" + cargo + "'";
            string rutBuscado = "";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                rutBuscado = (dr.GetString(0));
            }
            con.Close();
            return rutBuscado;
        }
        private string obtenerNombrePersonaByRut(string rut, string cargo)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT NOMBRE_COMPLETO FROM PERSONA WHERE RUT = '" + rut + "' AND CARGO = '" + cargo + "'";
            string buscado = "";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                buscado = (dr.GetString(0));

            }
            con.Close();
            return buscado;
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            List<string> rango_fecha = new List<string>();
            List<string> listado_horarios = new List<string>();
            List<int> id_trayectos = new List<int>();
            if (validarInsertViajeDiario())
            {

                DateTime inicio = DateTime.Parse(this.fechaDesde.Text);
                DateTime final = DateTime.Parse(this.fechaHasta.Text);

                for (DateTime i = inicio; i <= final; i = i.AddDays(1))
                {
                    rango_fecha.Add(i.ToString("dd-MM-yyyy"));

                }
                foreach (UIElement ui in this.horarios.Children)
                {


                    int row = System.Windows.Controls.Grid.GetRow(ui);
                    int col = System.Windows.Controls.Grid.GetColumn(ui);

                    if ((row == 0) && (col == 0 || col == 1 || col == 2))
                    {
                        Label label = (Label)ui;

                    }
                    else
                    {
                        TextBox txt = (TextBox)ui;

                        listado_horarios.Add(txt.Text);
                    }
                }
                string id_recorrido = lbl_id_recorrido.Text;
                string identificadorViaje = identificador.Text;
                string fechaInicioViaje = fechaDesde.Text;
                string fechaFinViaje = fechaHasta.Text;

                try
                {
                    // INSERT EN LA TABLA TRAYECTO GUARDO EL VIAJE
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    string insertString = "INSERT INTO viajes (RECORRIDO,IDENTIFICADOR,DESDE,HASTA) VALUES (?recorrido,?identificador,?desde,?hasta)";

                    MySqlCommand cmd = new MySqlCommand(insertString, con);
                    cmd.Parameters.Add("?recorrido", Convert.ToInt32(id_recorrido));
                    cmd.Parameters.Add("?identificador", identificadorViaje);
                    cmd.Parameters.Add("?desde", fechaInicioViaje);
                    cmd.Parameters.Add("?hasta", fechaFinViaje);

                    cmd.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    // OBTENGO LOS TRAYECTOS SEGUN EL VIAJE
                    string selectTrayectos = "SELECT ID FROM TRAYECTO WHERE RECORRIDO = " + lbl_id_recorrido.Text;

                    cmd = new MySqlCommand(selectTrayectos, con);

                    MySqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        id_trayectos.Add(dr.GetInt32(0));

                    }
                    con.Close();
                    con.Open();
                    int id_viaje = 0;
                    //OBTENGO EL ID DEL VIAJE CREADO.
                    string selectIDVIAJE = "SELECT ID FROM VIAJES WHERE RECORRIDO = " + lbl_id_recorrido.Text + " AND IDENTIFICADOR = '" + identificadorViaje + "'";
                    cmd = new MySqlCommand(selectIDVIAJE, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        id_viaje = (dr.GetInt32(0));

                    }

                    con.Close();


                    // INSERT EN VIAJES DIARIOS
                    string rutChofer = obtenerRutPersona(comboChofer.Text, "CHOFER");
                    string rutAuxiliar = obtenerRutPersona(comboAuxiliar.Text, "AUXILIAR");
                    for (int i = 0; i < rango_fecha.Count; i++)
                    {
                        for (int j = 0; j < id_trayectos.Count; j++)
                        {
                            con.Open();

                            insertString = "INSERT INTO viajes_diarios (VIAJE,TRAYECTO,FECHA,BUS,CHOFER,AUXILIAR,ASIENTOS_DISPONIBLES,RUTA_ARCHIVO) VALUES (?viaje,?trayecto,?fecha,?bus,?chofer,?auxiliar,45,'VACIO')";

                            cmd = new MySqlCommand(insertString, con);
                            cmd.Parameters.Add("?viaje", id_viaje);
                            cmd.Parameters.Add("?trayecto", id_trayectos[j]);
                            cmd.Parameters.Add("?fecha", rango_fecha[i]);
                            cmd.Parameters.Add("?bus", comboBus.Text);
                            cmd.Parameters.Add("?chofer", rutChofer);
                            cmd.Parameters.Add("?auxiliar", rutAuxiliar);

                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }


                    // INSERT EN HORARIOS
                    for (int i = 0; i < listado_horarios.Count; i = i + 3)
                    {
                        con.Open();
                        insertString = "INSERT INTO HORARIOS (VIAJE,LLEGADA,SALIDA,PARADA) VALUES (?viaje,?llegada,?salida,?parada)";

                        cmd = new MySqlCommand(insertString, con);
                        cmd.Parameters.Add("?viaje", id_viaje);
                        cmd.Parameters.Add("?llegada", listado_horarios[i]);
                        cmd.Parameters.Add("?salida", listado_horarios[i + 1]);
                        cmd.Parameters.Add("?parada", obtenerIdCiudad(listado_horarios[i + 2]));

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }


                    nuevoViaje nuevoViajeDiario = new nuevoViaje();
                    nuevoViajeDiario.Show();

                    comboDestino.Items.Clear();
                    identificador.Text = "";
                    datosViaje.Visibility = Visibility.Hidden;
                    comboOrigen.SelectedIndex = -1;
                } // FIN TRY
                catch (Exception ex)
                {
                    validar vInsert = new validar();
                    vInsert.show(ex.Message);
                }
            } // FIN IF DATOS VALIDOS
        }

        private void horarios_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void cOrigenEditar_Loaded(object sender, RoutedEventArgs e)
        {
            this.cOrigenEditar.Items.Clear();

            ciudadOrigen.Clear();
            ciudadesDistintas.Clear();
            ciudadDestino.Clear();
            id_recorrido.Clear();
            id_recorrido_seleccion.Clear();
            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                string sql = "SELECT MAX(RECORRIDO) FROM PARADA";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dr = cmd.ExecuteReader();

                int numeroRecorridos = 0;
                while (dr.Read())
                {
                    numeroRecorridos = dr.GetInt32(0);
                }

                con.Close();
                int i;
                // RECORRO LOS RECORRIDOS GENERALES.
                for (i = 0; i <= numeroRecorridos; i++)
                {
                    con.Open();
                    string sqlRecorridos = "SELECT NOMBRE FROM PARADA INNER JOIN CIUDAD ON PARADA.CIUDAD = CIUDAD.ID WHERE RECORRIDO = '" + i + "' LIMIT 1";
                    cmd = new MySqlCommand(sqlRecorridos, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        // GUARDO TODOS LOS ORIGENES
                        ciudadOrigen.Add(dr.GetString(0));
                        // ID RECORRIDO
                        id_recorrido.Add(i);
                    }
                    con.Close();
                }

                con.Open();
                string sqlciudadDestino = "SELECT NOMBRE FROM PARADA INNER JOIN CIUDAD ON PARADA.CIUDAD = CIUDAD.ID WHERE SIGUIENTE = -1";
                cmd = new MySqlCommand(sqlciudadDestino, con);

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ciudadDestino.Add(dr.GetString(0));
                }
                con.Close();

                ciudadesDistintas = ciudadOrigen.Distinct().ToList();

                for (int j = 0; j < ciudadesDistintas.Count; j++)
                {
                    cOrigenEditar.Items.Add(ciudadesDistintas[j]);
                }
            }
            catch (Exception ex)
            {
                validar v = new validar();
                v.show(ex.Message);
            }
        }

        private void cOrigenEditar_DropDownClosed(object sender, EventArgs e)
        {
            id_recorrido_seleccion.Clear();
            cDestinoEditar.Items.Clear();
            // OBTENER ID CIUDAD ELEGIDA.

            string ciudadOrigenElegida = this.cOrigenEditar.Text;

            try
            {
                // RECORRER LISTADO DE ORIGENES
                for (int i = 0; i < ciudadOrigen.Count; i++)
                {
                    if (ciudadOrigen[i].Equals(ciudadOrigenElegida))
                    {
                        id_recorrido_seleccion.Add(id_recorrido[i]);
                        this.cDestinoEditar.Items.Add(ciudadDestino[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                validar errorCiudad = new validar();
                errorCiudad.show(ex.Message);
            }
        }

        private void cDestinoEditar_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                int seleccion = this.cDestinoEditar.SelectedIndex;

                this.id_recorridoEditar.Text = Convert.ToString(this.id_recorrido_seleccion[seleccion]);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void verValores_Click(object sender, RoutedEventArgs e)
        {
            if (this.listParadas.Items.Count > 1)
            {
                Recorrido reco = this.recorridos[this.listRecorridos.SelectedIndex];
                List<string> listaParadas = new List<string>();
                string precio = " ";
                Trayecto trayecto;

                Parada parada = reco.parada;

                while (parada.id != -1)
                {
                    listaParadas.Add(parada.ciudad.nombre);
                    parada = parada.siguiente;
                }

                string[,] precios = new string[listaParadas.Count, listaParadas.Count];
                for (int i = 0; i < listaParadas.Count; i++)
                {
                    for (int j = 0; j < listaParadas.Count; j++)
                    {
                        trayecto = TrayectoFacade.buscarPorOrigenDestinoRecorrido(listaParadas[i], listaParadas[j], reco.id);

                        if (trayecto != null)
                        {
                            precio = trayecto.precio.ToString();
                        }
                        else
                        {
                            precio = " ";
                        }
                        precios[i, j] = precio;
                    }
                }
                tablaValores tv = new tablaValores(listaParadas, this.ciudades, precios);
                tv.ShowDialog();
            }
            else
            {
                validar alert = new validar();
                alert.show("El Recorrido debe tener como minimo dos Paradas");
            }
        }

        private void agregarValores_Click(object sender, RoutedEventArgs e)
        {
            if (this.listNuevoRecorrido.Items.Count > 1)
            {
                List<string> lp = new List<string>();
                for (int i = 0; i < this.listNuevoRecorrido.Items.Count; i++)
                {
                    lp.Add(this.listNuevoRecorrido.Items[i].ToString());
                }

                tablaValores tv = new tablaValores(lp, this.ciudades);
                bool? result = tv.ShowDialog();
                if (result == true)
                {
                    this.listNuevoRecorrido.Items.Clear();
                    this.listCiudad.Items.Clear();
                    foreach (KeyValuePair<string, int> city in this.ciudades)
                    {
                        this.listCiudad.Items.Add(city.Key);
                    }
                }
            }
            else
            {
                validar alert = new validar();
                alert.show("El Recorrido debe tener como minimo dos Paradas");
            }
        }

        private void comboOrigenReco_Loaded(object sender, RoutedEventArgs e)
        {
            this.comboOrigenReco.Items.Clear();
            List<Trayecto> origenes = TrayectoFacade.buscarOrigenes();
            foreach (Trayecto t in origenes)
            {
                this.comboOrigenReco.Items.Add(t.origen.nombre);
            }
        }

        private void comboOrigenReco_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboOrigenReco.SelectedIndex != -1)
            {
                List<Trayecto> destinos = TrayectoFacade.buscarDestinosPorOrigen(this.comboOrigenReco.SelectedItem.ToString());
                this.comboDestinoReco.Items.Clear();
                foreach (Trayecto d in destinos)
                {
                    this.comboDestinoReco.Items.Add(d.destino.nombre);
                }
            }
        }

        private void comboDestinoReco_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.listRecorridos.Items.Clear();
            try
            {
                this.recorridos = RecorridoFacade.buscarPorOrigenDestino(this.comboOrigenReco.SelectedItem.ToString(), this.comboDestinoReco.SelectedItem.ToString());
                if (this.recorridos.Count > 0)
                {
                    foreach (Recorrido rec in this.recorridos)
                    {                        
                        Parada p = rec.parada;
                        while (p.id != -1)
                        {
                            if (p.siguiente.id == -1)
                            {
                                break;
                            }
                            else
                            {
                                p = p.siguiente;
                            }
                        }
                        this.listRecorridos.Items.Add(rec.id + ". " + rec.parada.ciudad.nombre + " - " + p.ciudad.nombre);
                    }
                }
                else
                {
                    validar alert = new validar();
                    alert.show("No hay recorridos asociados");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("comboDestinoReco_SelectionChanged: " + ex.Message);
            }
        }
    }
}
