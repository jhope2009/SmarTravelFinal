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

        List<Recorrido> listadoId = new List<Recorrido>();
        public static agregarViajeDiario addViaje = null;

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
            if (comboOrigen.SelectedIndex == -1)
            {

                mensajeValidacion.show("Seleccione el origen del destino");
                return false;
            }
            else if (comboDestino.SelectedIndex == -1)
            {
                mensajeValidacion.show("Seleccione el destino del viaje.");
                return false;
            }
            return true;


        }
        private Boolean validarEditViajeDiario()
        {
            validar mensajeValidacion = new validar();
            if (cOrigenEditar.SelectedIndex == -1)
            {

                mensajeValidacion.show("Seleccione el origen del destino");
                return false;
            }
            else if (cDestinoEditar.SelectedIndex == -1)
            {
                mensajeValidacion.show("Seleccione el destino del viaje.");
                return false;
            }
            return true;


        }
        private void cabecerasAgregarViajes() {
            gridAgregarViajes.Children.Clear();
            gridAgregarViajes.ColumnDefinitions.Clear();
            gridAgregarViajes.RowDefinitions.Clear();

            this.gridAgregarViajes.RowDefinitions.Add(new RowDefinition());
            // Intermedios
            this.gridAgregarViajes.ColumnDefinitions.Add(new ColumnDefinition());
            // BOTON
            this.gridAgregarViajes.ColumnDefinitions.Add(new ColumnDefinition());
            
            Label intermediosHeader = new Label();
            intermediosHeader.Content = "INTERMEDIOS";
            intermediosHeader.Style = Resources["HeaderTabla"] as Style;

            intermediosHeader.SetValue(Grid.ColumnProperty, 0);
            intermediosHeader.SetValue(Grid.RowProperty, 0);
            this.gridAgregarViajes.Children.Add(intermediosHeader);

            Label buttonHeader = new Label();
            buttonHeader.Content = " ";
            buttonHeader.Style = Resources["HeaderTabla"] as Style;
            buttonHeader.Width = 100;
            buttonHeader.SetValue(Grid.ColumnProperty, 1);
            buttonHeader.SetValue(Grid.RowProperty, 0);
            this.gridAgregarViajes.Children.Add(buttonHeader);

        
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (validarIngresoViajeDiario())
            {
                //listadoId.Clear();
                cabecerasAgregarViajes();
                this.gridAgregarViajes.Visibility = Visibility.Visible;
                Ciudad origen = CiudadFacade.buscarPorNombre(this.comboOrigen.SelectedItem.ToString());
                Ciudad destino = CiudadFacade.buscarPorNombre(this.comboDestino.SelectedItem.ToString());

                listadoId = RecorridoFacade.RecorridoByOrigenDestino(origen, destino);
                List<string> nombreIntermedios = new List<string>();

                for (int z = 0; z < listadoId.Count; z++)
                {
                    nombreIntermedios.Add(ParadaFacade.nombresIntermediosByRecorridos(listadoId[z].id));

                }

                int largo = listadoId.Count - 1;
                this.gridAgregarViajes.ColumnDefinitions[0].Width = new System.Windows.GridLength(500);
                this.gridAgregarViajes.ColumnDefinitions[1].Width = new System.Windows.GridLength(80);
                for (int z = 0; z <= largo; z++)
                {
                    this.gridAgregarViajes.RowDefinitions.Add(new RowDefinition());
                    this.gridAgregarViajes.RowDefinitions[z].Height = new System.Windows.GridLength(40);

                }
                this.gridAgregarViajes.RowDefinitions[gridAgregarViajes.RowDefinitions.Count-1].Height = new System.Windows.GridLength(40);
                int contador = 0;
                TextBox texto;
                for (int row = 1; row < this.gridAgregarViajes.RowDefinitions.Count; row++)
                {
                    for (int col = 0; col < this.gridAgregarViajes.ColumnDefinitions.Count; col++)
                    {
                        if (col == 0)
                        {

                            texto = new TextBox();
                            texto.Style = Resources["ItemTablaGuion"] as Style;

                            texto.Text = nombreIntermedios[contador];
                            texto.SetValue(Grid.ColumnProperty, col);
                            texto.SetValue(Grid.RowProperty, row);
                            texto.IsReadOnly = true;
                            this.gridAgregarViajes.Children.Add(texto);
                        }

                        if (col == 1)
                        {
                            BitmapImage btm = new BitmapImage(new Uri("/SmarTravel_Final;component/Images/crear.png", UriKind.Relative));
                            Image img = new Image();
                            img.Source = btm;
                            img.Stretch = Stretch.Fill;
                            img.Width = 50;
                            img.Height = 30;
                            Button bIngreso = new Button();
                            bIngreso.Click += new RoutedEventHandler(bIngreso_Click);
                            bIngreso.Content = img;
                            bIngreso.Tag = Convert.ToString(contador);
                            bIngreso.SetValue(Grid.ColumnProperty, col);
                            bIngreso.SetValue(Grid.RowProperty, row);
                             this.gridAgregarViajes.Children.Add(bIngreso);

                        }
                    }
                    contador++;
                }

            }
        }

       
        private void tabViajesDiarios_Selected(object sender, RoutedEventArgs e)
        {
            this.gridAgregarViajes.Visibility = Visibility.Hidden;
            this.comboOrigen.Items.Clear();
            this.comboDestino.Items.Clear();


            List<Parada> AllCiudadesGenerales = ParadaFacade.buscarOrigenesGenerales();
            foreach (Parada ciudadOrigen in AllCiudadesGenerales)
            {
                Boolean validarCiudades = true;;

                for (int i = 0; i < this.comboOrigen.Items.Count; i++)
                {
                    if (ciudadOrigen.ciudad.nombre.Equals(this.comboOrigen.Items.GetItemAt(i))) {

                        validarCiudades = false;
                    }
                }
                if (validarCiudades)
                {
                    this.comboOrigen.Items.Add(ciudadOrigen.ciudad.nombre);
                }
            }

        }
        private void tabEditarViajes_Selected(object sender, RoutedEventArgs e)
        {
            this.listadoRecorridos.Visibility = Visibility.Hidden;
            this.cOrigenEditar.Items.Clear();
            this.cDestinoEditar.Items.Clear();

            List<Parada> AllCiudadesGenerales = ParadaFacade.buscarOrigenesGenerales();
            foreach (Parada ciudadOrigen in AllCiudadesGenerales)
            {
                Boolean validarCiudades = true; ;

                for (int i = 0; i < this.cOrigenEditar.Items.Count; i++)
                {
                    if (ciudadOrigen.ciudad.nombre.Equals(this.cOrigenEditar.Items.GetItemAt(i)))
                    {

                        validarCiudades = false;
                    }
                }
                if (validarCiudades)
                {
                    this.cOrigenEditar.Items.Add(ciudadOrigen.ciudad.nombre);
                }
            }
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
            if (validarEditViajeDiario())
            {

                listadoRecorridos.RowDefinitions.Clear();
                listadoRecorridos.ColumnDefinitions.Clear();
                this.listadoRecorridos.Visibility = Visibility.Visible;
                
                List<string> listHorarioSalida = new List<string>();
                List<string> listHorarioLlegada = new List<string>();

                Ciudad origen = CiudadFacade.buscarPorNombre(this.cOrigenEditar.Text);
                Ciudad destino = CiudadFacade.buscarPorNombre(this.cDestinoEditar.Text);
                List<Viaje> allViajesByOrigenDestino = ViajeFacade.buscarViajePorOrigenAndDestino(origen, destino);
                try
                {

                    if (allViajesByOrigenDestino.Count > -1)
                    {

                        // Obtener HORARIO DE SALIDA Y LLEGADA
                        for (int i = 0; i < allViajesByOrigenDestino.Count; i++)
                        {

                            MySqlConnection con = conexionDB.ObtenerConexion();
                            string sql = "SELECT SALIDA FROM HORARIOS WHERE LLEGADA = '-' AND VIAJE =" + allViajesByOrigenDestino[i].id;
                            MySqlCommand cmd = new MySqlCommand(sql, con);

                            MySqlDataReader dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                listHorarioSalida.Add(dr.GetString(0));

                            }
                            con.Close();

                            con.Open();
                            string sqlHoraLlegada = "SELECT LLEGADA FROM HORARIOS WHERE SALIDA = '-' AND VIAJE =" + allViajesByOrigenDestino[i].id;

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
                        int largo = allViajesByOrigenDestino.Count;
                        this.listadoRecorridos.ColumnDefinitions[0].Width = new System.Windows.GridLength(180);
                        this.listadoRecorridos.ColumnDefinitions[1].Width = new System.Windows.GridLength(185);
                        this.listadoRecorridos.ColumnDefinitions[2].Width = new System.Windows.GridLength(120);
                        this.listadoRecorridos.ColumnDefinitions[3].Width = new System.Windows.GridLength(60);

                        for (int z = 0; z < largo; z++)
                        {

                            this.listadoRecorridos.RowDefinitions.Add(new RowDefinition());
                            this.listadoRecorridos.RowDefinitions[z].Height = new System.Windows.GridLength(40);

                        }
                        this.listadoRecorridos.RowDefinitions[0].Height = new System.Windows.GridLength(30);
                        this.listadoRecorridos.RowDefinitions[listadoRecorridos.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(40);


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
                                    tHorarios.Text = origen.nombre + "- " + destino.nombre;
                                    tHorarios.SetValue(Grid.ColumnProperty, col);
                                    tHorarios.SetValue(Grid.RowProperty, row);
                                    tHorarios.IsReadOnly = true;
                                    this.listadoRecorridos.Children.Add(tHorarios);

                                }
                                if (col == 1)
                                {
                                    tHorarios = new TextBox();
                                    tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                                    tHorarios.Text = allViajesByOrigenDestino[contador].identificador;
                                    tHorarios.SetValue(Grid.ColumnProperty, col);
                                    tHorarios.SetValue(Grid.RowProperty, row);
                                    tHorarios.IsReadOnly = true;
                                    this.listadoRecorridos.Children.Add(tHorarios);

                                }
                                if (col == 2)
                                {
                                    tHorarios = new TextBox();
                                    tHorarios.Style = Resources["ItemTablaGuion"] as Style;

                                    tHorarios.Text = listHorarioSalida[contador] + " - " + listHorarioLlegada[contador];
                                    tHorarios.SetValue(Grid.ColumnProperty, col);
                                    tHorarios.SetValue(Grid.RowProperty, row);
                                    tHorarios.IsReadOnly = true;
                                    this.listadoRecorridos.Children.Add(tHorarios);

                                }
                                if (col == 3)
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

                                    b.Tag = Convert.ToString(allViajesByOrigenDestino[contador].id);
                                    b.SetValue(Grid.ColumnProperty, col);
                                    b.SetValue(Grid.RowProperty, row);


                                    this.listadoRecorridos.Children.Add(b);

                                }
                            } // Fin columnas
                            contador++;

                        }// FIn tabla
                    }

                    else
                    {
                        validar v = new validar();
                        v.show("No hay viajes diarios creados");
                    }
                    

                    
                } // Fin try
                catch (Exception ex)
                {
                    validar v = new validar();
                    v.show(ex.Message);
                }
                
            } // Fin validar datos
        

                    

        }
        public void bIngreso_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var fila = button.Tag;       
            
            addViaje = new agregarViajeDiario();
            addViaje.Show();

            addViaje.getIdRecorrido(listadoId[Convert.ToInt32(fila)].id);
            addViaje.crearTabla();
            


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

           
            this.listadoRecorridos.RowDefinitions.Add(new RowDefinition());

            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());
            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());
            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());
            this.listadoRecorridos.ColumnDefinitions.Add(new ColumnDefinition());

            Label origenDestinoHeader = new Label();
            origenDestinoHeader.Content = "ORIGEN - DESTINO";
            origenDestinoHeader.Style = Resources["HeaderTabla"] as Style;

            origenDestinoHeader.SetValue(Grid.ColumnProperty, 0);
            origenDestinoHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(origenDestinoHeader);


            Label identificadorHeader = new Label();
            identificadorHeader.Content = "IDENTIFICADOR";
            identificadorHeader.Style = Resources["HeaderTabla"] as Style;
            identificadorHeader.Width = 200;
            identificadorHeader.SetValue(Grid.ColumnProperty, 1);
            identificadorHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(identificadorHeader);

            Label horarioHeader = new Label();
            horarioHeader.Content = "HORARIO";
            horarioHeader.Style = Resources["HeaderTabla"] as Style;
            horarioHeader.Width = 200;
            horarioHeader.SetValue(Grid.ColumnProperty, 2);
            horarioHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(horarioHeader);

            Label verHeader = new Label();
            verHeader.Content = "";
            verHeader.Style = Resources["HeaderTabla"] as Style;
            verHeader.Width = 100;
            verHeader.SetValue(Grid.ColumnProperty,3);
            verHeader.SetValue(Grid.RowProperty, 0);
            this.listadoRecorridos.Children.Add(verHeader);
            //int precio = this.listadoRecorridos.Children.Count;
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
       
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            
           
        }

        private void horarios_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

      
        private void cOrigenEditar_DropDownClosed(object sender, EventArgs e)
        {
            cDestinoEditar.Items.Clear();
            // OBTENER ID CIUDAD ELEGIDA.

            List<int> recorridos = new List<int>();

            string ciudadOrigenElegida = this.cOrigenEditar.Text;
            Ciudad origen = CiudadFacade.buscarCiudadPorNombre(ciudadOrigenElegida);

            List<Parada> Origenes = ParadaFacade.buscarOrigenesGenerales();
            foreach (Parada ciudadesOrigenes in Origenes)
            {
                if (ciudadesOrigenes.ciudad.nombre.Equals(ciudadOrigenElegida))
                {
                    recorridos.Add(ciudadesOrigenes.recorrido);
                }

            }

            List<Parada> destinos = ParadaFacade.buscarDestinosGeneralesByCiudad(origen, recorridos);

            foreach (Parada ciudad in destinos)
            {
                Boolean validarCiudades = true; ;

                for (int i = 0; i < this.cDestinoEditar.Items.Count; i++)
                {
                    if (ciudad.ciudad.nombre.Equals(this.cDestinoEditar.Items.GetItemAt(i)))
                    {

                        validarCiudades = false;
                    }
                }
                if (validarCiudades)
                {
                    this.cDestinoEditar.Items.Add(ciudad.ciudad.nombre);
                }
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

  

        private void comboOrigen_DropDownClosed(object sender, EventArgs e)
        {
            comboDestino.Items.Clear();
            // OBTENER ID CIUDAD ELEGIDA.

            List<int> recorridos = new List<int>();

            string ciudadOrigenElegida = this.comboOrigen.Text;
            Ciudad origen = CiudadFacade.buscarCiudadPorNombre(ciudadOrigenElegida);

            List<Parada> Origenes = ParadaFacade.buscarOrigenesGenerales();
            foreach (Parada ciudadesOrigenes in Origenes)
            {
                if (ciudadesOrigenes.ciudad.nombre.Equals(ciudadOrigenElegida))
                {
                    recorridos.Add(ciudadesOrigenes.recorrido);
                }

            }
            
            List<Parada> destinos = ParadaFacade.buscarDestinosGeneralesByCiudad(origen,recorridos);

            foreach (Parada ciudad in destinos)
            {
                Boolean validarCiudades = true; ;

                for (int i = 0; i < this.comboDestino.Items.Count; i++)
                {
                    if (ciudad.ciudad.nombre.Equals(this.comboDestino.Items.GetItemAt(i)))
                    {

                        validarCiudades = false;
                    }
                }
                if (validarCiudades)
                {
                    this.comboDestino.Items.Add(ciudad.ciudad.nombre);
                }
            }
                                                               
        }

        
        public void formatoTablaViaje(){

            this.ListadoViajes.Children.Clear();
            this.ListadoViajes.RowDefinitions.Clear();
            this.ListadoViajes.ColumnDefinitions.Clear();
            this.ListadoViajes.RowDefinitions.Add(new RowDefinition());

            this.ListadoViajes.ColumnDefinitions.Add(new ColumnDefinition());
            this.ListadoViajes.ColumnDefinitions.Add(new ColumnDefinition());
            this.ListadoViajes.ColumnDefinitions.Add(new ColumnDefinition());
            this.ListadoViajes.ColumnDefinitions.Add(new ColumnDefinition());
            this.ListadoViajes.ColumnDefinitions.Add(new ColumnDefinition());
            this.ListadoViajes.ColumnDefinitions.Add(new ColumnDefinition());

            Label intermediosHeader = new Label();
            intermediosHeader.Content = "VIAJE";
            intermediosHeader.Style = Resources["HeaderTabla"] as Style;

            intermediosHeader.SetValue(Grid.ColumnProperty, 0);
            intermediosHeader.SetValue(Grid.RowProperty, 0);
            this.ListadoViajes.Children.Add(intermediosHeader);


            Label horarioHeader = new Label();
            horarioHeader.Content = "HORARIO";
            horarioHeader.Style = Resources["HeaderTabla"] as Style;
            horarioHeader.Width = 200;
            horarioHeader.SetValue(Grid.ColumnProperty, 1);
            horarioHeader.SetValue(Grid.RowProperty, 0);
            this.ListadoViajes.Children.Add(horarioHeader);

            Label identificadorHeader = new Label();
            identificadorHeader.Content = "IDENTIFICADOR";
            identificadorHeader.Style = Resources["HeaderTabla"] as Style;
            identificadorHeader.Width = 200;
            identificadorHeader.SetValue(Grid.ColumnProperty, 2);
            identificadorHeader.SetValue(Grid.RowProperty, 0);
            this.ListadoViajes.Children.Add(identificadorHeader);

            Label desdeHeader = new Label();
            desdeHeader.Content = "DESDE";
            desdeHeader.Style = Resources["HeaderTabla"] as Style;
            desdeHeader.Width = 200;
            desdeHeader.SetValue(Grid.ColumnProperty, 3);
            desdeHeader.SetValue(Grid.RowProperty, 0);
            this.ListadoViajes.Children.Add(desdeHeader);

            Label hastaHeader = new Label();
            hastaHeader.Content = "HASTA";
            hastaHeader.Style = Resources["HeaderTabla"] as Style;
            hastaHeader.Width = 200;
            hastaHeader.SetValue(Grid.ColumnProperty, 4);
            hastaHeader.SetValue(Grid.RowProperty, 0);
            this.ListadoViajes.Children.Add(hastaHeader);

            Label verHeader = new Label();
            verHeader.Content = "";
            verHeader.Style = Resources["HeaderTabla"] as Style;
            verHeader.Width = 100;
            verHeader.SetValue(Grid.ColumnProperty, 5);
            verHeader.SetValue(Grid.RowProperty, 0);
            this.ListadoViajes.Children.Add(verHeader);


            //int largo = paradas.Count;
            this.ListadoViajes.ColumnDefinitions[0].Width = new System.Windows.GridLength(210);
            this.ListadoViajes.ColumnDefinitions[1].Width = new System.Windows.GridLength(130);
            this.ListadoViajes.ColumnDefinitions[2].Width = new System.Windows.GridLength(210);
            this.ListadoViajes.ColumnDefinitions[3].Width = new System.Windows.GridLength(110);
            this.ListadoViajes.ColumnDefinitions[4].Width = new System.Windows.GridLength(110);
            this.ListadoViajes.ColumnDefinitions[5].Width = new System.Windows.GridLength(90);
            
            
        }
        private void tabEditarViajeDiario_Selected(object sender, RoutedEventArgs e)
        {
            formatoTablaViaje();
            List<Viaje> allViajes = ViajeFacade.buscarAllViajes();
            List<string> listHorarioSalida = new List<string>();
            List<string> listHorarioLlegada = new List<string>();
            int largo = allViajes.Count;
            for (int z = 0; z < largo; z++)
            {
                this.ListadoViajes.RowDefinitions.Add(new RowDefinition());

                this.ListadoViajes.RowDefinitions[z].Height = new System.Windows.GridLength(40);
            }
            this.ListadoViajes.RowDefinitions[0].Height = new System.Windows.GridLength(25);
            this.ListadoViajes.RowDefinitions[ListadoViajes.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(40);


            List<string> nombreIntermedios = new List<string>();
            foreach (Viaje viaje in allViajes)
            {
                List<Recorrido> listado = RecorridoFacade.OrigenDestinoByRecorrido(viaje.reco);
                foreach (Recorrido ciudad in listado)
                {
                    // TENGO LOS ORIGENES Y DESTINOS DE LOS VIAJES
                    nombreIntermedios.Add(ciudad.origen.nombre + " - " + ciudad.destino.nombre);
                }
            }

            if (allViajes.Count > -1)
            {
                
                // Obtener HORARIO DE SALIDA Y LLEGADA
                for (int i = 0; i < allViajes.Count; i++)
                {

                    MySqlConnection con = conexionDB.ObtenerConexion();   
                    string sql = "SELECT SALIDA FROM HORARIOS WHERE LLEGADA = '-' AND VIAJE =" + allViajes[i].id;
                    MySqlCommand cmd = new MySqlCommand(sql, con);

                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        listHorarioSalida.Add(dr.GetString(0));

                    }

                    dr.Close();
                    con.Close();
                    con.Open();
                    string sqlHoraLlegada = "SELECT LLEGADA FROM HORARIOS WHERE SALIDA = '-' AND VIAJE =" + allViajes[i].id;

                    MySqlCommand cmd2 = new MySqlCommand(sqlHoraLlegada, con);

                    MySqlDataReader dr2 = cmd2.ExecuteReader();

                    while (dr2.Read())
                    {

                        listHorarioLlegada.Add(dr2.GetString(0));


                    }
                    con.Close();

                }


                TextBox celda;
                int contador = 0;
                for (int row = 1; row < this.ListadoViajes.RowDefinitions.Count; row++)
                {
                    for (int col = 0; col < this.ListadoViajes.ColumnDefinitions.Count; col++)
                    {

                        if (col == 0)
                        {
                            celda = new TextBox();
                            celda.Style = Resources["ItemTablaGuion"] as Style;
                            celda.Text = nombreIntermedios[contador];
                            celda.SetValue(Grid.ColumnProperty, col);
                            celda.SetValue(Grid.RowProperty, row);
                            celda.IsReadOnly = true;
                            this.ListadoViajes.Children.Add(celda);

                        }
                        if (col == 1)
                        {
                            celda = new TextBox();
                            celda.Style = Resources["ItemTablaGuion"] as Style;
                            celda.Text = listHorarioSalida[contador] +" - "+ listHorarioLlegada[contador];
                            celda.SetValue(Grid.ColumnProperty, col);
                            celda.SetValue(Grid.RowProperty, row);
                            celda.IsReadOnly = true;
                            this.ListadoViajes.Children.Add(celda);

                        }
                        if (col == 2)
                        {
                            celda = new TextBox();
                            celda.Style = Resources["ItemTablaGuion"] as Style;
                            celda.Text = allViajes[contador].identificador;
                            celda.SetValue(Grid.ColumnProperty, col);
                            celda.SetValue(Grid.RowProperty, row);
                            celda.IsReadOnly = true;
                            this.ListadoViajes.Children.Add(celda);

                        }
                        if (col == 3)
                        {
                            celda = new TextBox();
                            celda.Style = Resources["ItemTablaGuion"] as Style;

                            celda.Text = allViajes[contador].fechaDesde;
                            celda.SetValue(Grid.ColumnProperty, col);
                            celda.SetValue(Grid.RowProperty, row);
                            celda.IsReadOnly = true;
                            this.ListadoViajes.Children.Add(celda);

                        }
                        if (col == 4)
                        {
                            celda = new TextBox();
                            celda.Style = Resources["ItemTablaGuion"] as Style;

                            celda.Text = allViajes[contador].fechaHasta;
                            celda.SetValue(Grid.ColumnProperty, col);
                            celda.SetValue(Grid.RowProperty, row);
                            celda.IsReadOnly = true;
                            this.ListadoViajes.Children.Add(celda);

                        }
                        if (col == 5)
                        {
                            BitmapImage btm = new BitmapImage(new Uri("/SmarTravel_Final;component/Images/busquedaIcon.png", UriKind.Relative));
                            Image img = new Image();
                            img.Source = btm;
                            img.Stretch = Stretch.Fill;
                            img.Width = 30;
                            img.Height = 30;
                            Button buttonNewViaje = new Button();
                            buttonNewViaje.Click += new RoutedEventHandler(buttonNewViaje_Click);

                            //b.Content = "VER"
                            buttonNewViaje.Content = img;

                            buttonNewViaje.Tag = Convert.ToString(allViajes[contador].id);
                            buttonNewViaje.SetValue(Grid.ColumnProperty, col);
                            buttonNewViaje.SetValue(Grid.RowProperty, row);


                            this.ListadoViajes.Children.Add(buttonNewViaje);

                        }

                    }
                    contador++;

                }
            }


        } // Fin metodo


        public void buttonNewViaje_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var fila = button.Tag;

            editarViajeDiario edit = new editarViajeDiario ();
            edit.Show();

            edit.getIdViaje(Convert.ToInt32(fila.ToString()));
            edit.crearTabla();


        }




    }
}
