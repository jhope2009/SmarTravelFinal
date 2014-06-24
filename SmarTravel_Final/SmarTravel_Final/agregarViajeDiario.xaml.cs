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
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using SmarTravel_Final.Controller;

namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para agregarViajeDiario.xaml
    /// </summary>
    public partial class agregarViajeDiario : Window
    {
        //List<string> paradas = new List<string>();
        int recorrido;
        List<string> listHorarios = new List<string>();
        List<string> listHorariosUpdate = new List<string>();
        public agregarViajeDiario()
        {
            InitializeComponent();
        }

        public void getIdRecorrido(int reco)
        {
            this.recorrido = reco;
        }
        public void crearTabla()
        {
            horarios.Children.Clear();
            horarios.ColumnDefinitions.Clear();
            horarios.RowDefinitions.Clear();
            //paradas.Clear();
            TextBox tHorarios;
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

                    List<Parada> paradas = ParadaFacade.buscarCiudadesPorRecorrido(this.recorrido);
                    /*
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    string sql = "SELECT CIUDAD FROM PARADA WHERE RECORRIDO = '" + this.recorrido + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        paradas.Add(dr.GetString(0));


                    }
                    con.Close();
                     * */

                    int largo = paradas.Count;
                    this.horarios.ColumnDefinitions[0].Width = new System.Windows.GridLength(70);
                    this.horarios.ColumnDefinitions[1].Width = new System.Windows.GridLength(70);
                    for (int z = 0; z < largo; z++)
                    {
                        this.horarios.RowDefinitions.Add(new RowDefinition());
                        
                        this.horarios.RowDefinitions[z].Height = new System.Windows.GridLength(40);
                    }
                    this.horarios.RowDefinitions[horarios.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(40);
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
                                    
                                    tHorarios = new TextBox();
                                    tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                                    //tHorarios.Text = ciudadBuscada;
                                    tHorarios.Text = paradas[contador].ciudad.nombre;
                                    tHorarios.SetValue(Grid.ColumnProperty, col);
                                    tHorarios.SetValue(Grid.RowProperty, row);
                                    this.horarios.Children.Add(tHorarios);

                                    //con.Close();
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
        private Boolean validarInsertViajeDiario()
        {
            
            validar mensajeValidacion = new validar();
            string fechaActual = DateTime.Today.ToString("dd-MM-yyyy");

            if (identificador.Text == "") {
                mensajeValidacion.show("Ingrese el identificador del viaje.");
                return false;
            }
            else if (comboChofer.SelectedIndex == -1)
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
        private void crear_Click(object sender, RoutedEventArgs e)
        {
             
                List<string> rango_fecha = new List<string>();
                List<string> listado_horarios = new List<string>();
               // List<int> id_trayectos = new List<int>();
                if (validarInsertViajeDiario())
                    {

                        DateTime inicio = DateTime.Parse(this.fechaDesde.Text);
                        DateTime final = DateTime.Parse(this.fechaHasta.Text);

                        for (DateTime i = inicio; i <= final; i = i.AddDays(1)){
                            rango_fecha.Add(i.ToString("dd-MM-yyyy"));

                        }
                       foreach (UIElement ui in this.horarios.Children)
                        {

                            int row = System.Windows.Controls.Grid.GetRow(ui);
                            int col = System.Windows.Controls.Grid.GetColumn(ui);

                           if ((row == 0) && (col == 0 || col == 1 || col == 2)){
                                Label label = (Label)ui;

                                }
                           else
                            {
                                TextBox txt = (TextBox)ui;
                                listado_horarios.Add(txt.Text);
                            }
                    }

                    string id_recorrido = this.recorrido.ToString();
                    string identificadorViaje = identificador.Text;
                    string fechaInicioViaje = fechaDesde.Text;
                    string fechaFinViaje = fechaHasta.Text;

                    try{
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

                    //con.Open();


                    List<Trayecto> id_trayectos = TrayectoFacade.buscarPorRecorrido(this.recorrido);
                    // OBTENGO LOS TRAYECTOS SEGUN EL VIAJE
                    
                   /*string selectTrayectos = "SELECT ID FROM TRAYECTO WHERE RECORRIDO = " + this.recorrido;

                    cmd = new MySqlCommand(selectTrayectos, con);

                    MySqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        id_trayectos.Add(dr.GetInt32(0));

                    }
                    con.Close();*/
                    con.Open();
                    int id_viaje = 0;
                    //OBTENGO EL ID DEL VIAJE CREADO.
                    string selectIDVIAJE = "SELECT ID FROM VIAJES WHERE RECORRIDO = " + this.recorrido + " AND IDENTIFICADOR = '" + identificadorViaje + "'";
                    cmd = new MySqlCommand(selectIDVIAJE, con);
                    MySqlDataReader dr = cmd.ExecuteReader();
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
                            cmd.Parameters.Add("?trayecto", id_trayectos[j].id);
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
                        
                        Parada paradaCiudad  = ParadaFacade.buscarPorRecorridoCiudad(this.recorrido,(listado_horarios[i + 2]));
                        cmd.Parameters.Add("?parada", paradaCiudad.id);

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    nuevoViaje nuevoViajeDiario = new nuevoViaje();
                    nuevoViajeDiario.Show();
                } // FIN TRY
                catch (Exception ex)
                {
                    validar vInsert = new validar();
                    vInsert.show(ex.Message);
                }
            } // FIN IF DATOS VALIDOS
            
            
            
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

        private void comboBus_Loaded(object sender, RoutedEventArgs e)
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



        }
    }

