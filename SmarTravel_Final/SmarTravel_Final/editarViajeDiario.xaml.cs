﻿using System;
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
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Collections.ObjectModel;
using SmarTravel_Final.Controller;
namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para editarViajeDiario.xaml
    /// </summary>
    public partial class editarViajeDiario : Window
    {
        int id_viaje = 0;
        List<string> paradas = new List<string>();
        List<string> listHorarios = new List<string>();
        List<string> listHorariosLlegadaUpdate = new List<string>();
        List<string> listHorariosSalidaUpdate = new List<string>();
        public static nuevoViaje nuevoViajeDiario = null;
        public editarViajeDiario()
        {
            InitializeComponent();
        }

        public void getIdViaje(int viaje)
        {
            this.id_viaje = viaje;
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
        private string obtenerRutPersonaByNombre(string nombre, string cargo)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string sql = "SELECT RUT FROM PERSONA WHERE NOMBRE_COMPLETO = '" + nombre + "' AND CARGO = '" + cargo + "'";
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

        public void crearTabla() { 
            horarios.RowDefinitions.Clear();
            paradas.Clear();
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


                    MySqlConnection con = conexionDB.ObtenerConexion();
                    string sql = "SELECT LLEGADA,SALIDA,P.CIUDAD FROM HORARIOS AS H INNER JOIN PARADA AS P ON H.PARADA = P.ID WHERE VIAJE = " + this.id_viaje;
                    
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {

                        if (!dr.GetString(0).Equals("-"))
                        {
                            listHorarios.Add(dr.GetString(0));
                        }
                        if (!dr.GetString(1).Equals("-"))
                        {
                            listHorarios.Add(dr.GetString(1));
                        }

                        Ciudad parada = CiudadFacade.buscarPorId(dr.GetInt32(2));
                        paradas.Add(parada.nombre);
                       

                    }
                    con.Close();


                    this.horarios.ColumnDefinitions[0].Width = new System.Windows.GridLength(120);
                    this.horarios.ColumnDefinitions[1].Width = new System.Windows.GridLength(120);
                    this.horarios.ColumnDefinitions[2].Width = new System.Windows.GridLength(220);
                    int largo = paradas.Count;
                    for (int z = 0; z < largo; z++)
                    {

                        //this.horarios.ColumnDefinitions.Add(new ColumnDefinition());
                        this.horarios.RowDefinitions.Add(new RowDefinition());
                       
                        this.horarios.RowDefinitions[z].Height = new System.Windows.GridLength(50);
                    }
                    this.horarios.RowDefinitions[0].Height = new System.Windows.GridLength(30);
                    this.horarios.RowDefinitions[horarios.RowDefinitions.Count-1].Height = new System.Windows.GridLength(50);
                    int contador = 0;
                    int registrohorario = 0;
                    for (int row = 1; row < this.horarios.RowDefinitions.Count; row++)
                    {
                        for (int col = 0; col < this.horarios.ColumnDefinitions.Count; col++)
                        {
                            if ((col == 0 && row == 1) || (col == 1 && row == horarios.RowDefinitions.Count-1))
                            {
                                tHorarios = new TextBox();
                                tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                                tHorarios.Text = "-";
                                tHorarios.SetValue(Grid.ColumnProperty, col);
                                tHorarios.SetValue(Grid.RowProperty, row);
                                tHorarios.IsReadOnly = true;
                                this.horarios.Children.Add(tHorarios);
                            }
                            else if (col == 2 && row != 0)
                            {
                                if (contador < paradas.Count)
                                {

                                 
                                    tHorarios = new TextBox();
                                    tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                                    tHorarios.Text = paradas[contador];
                                    tHorarios.SetValue(Grid.ColumnProperty, col);
                                    tHorarios.SetValue(Grid.RowProperty, row);
                                    tHorarios.IsReadOnly = true;
                                    this.horarios.Children.Add(tHorarios);

                                    
                                }
                            }
                            else {
                                tHorarios = new TextBox();
                                tHorarios.Style = Resources["ItemTablaGuion"] as Style;
                                tHorarios.Text = listHorarios[registrohorario];
                                tHorarios.MaxLength = 5;
                                tHorarios.IsReadOnly = false;
                                tHorarios.SetValue(Grid.ColumnProperty, col);
                                tHorarios.SetValue(Grid.RowProperty, row);
                                this.horarios.Children.Add(tHorarios);
                                registrohorario++;
                            }
                        }
                        contador++;
                    }
                   //MessageBox.Show("Childe"+this.horarios.Children.Count);
                con.Open();
                sql = "SELECT BUS,CHOFER,AUXILIAR,DESDE,HASTA FROM VIAJES_DIARIOS AS VD INNER JOIN VIAJES AS V ON VD.VIAJE=V.ID WHERE VIAJE = "+this.id_viaje+" limit 1";
                cmd = new MySqlCommand(sql, con);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    this.comboBus.Text = dr.GetString(0);
                    this.fechaDesde.Text = dr.GetString(3);
                    this.fechaHasta.Text = dr.GetString(4);
                    this.comboAuxiliar.Text = obtenerNombrePersonaByRut(dr.GetString(2),"AUXILIAR");
                    this.comboChofer.Text = obtenerNombrePersonaByRut(dr.GetString(1), "CHOFER");
                }
                con.Close();
               } // Fin TRY



                catch (Exception ex)
                {
                    validar validarError = new validar();
                    validarError.show(ex.Message);
                }
            }

        private void comboChofer_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void comboChofer_Loaded_1(object sender, RoutedEventArgs e)
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
        private Boolean validarUpdate() {
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

            
        
        private void editar_Click(object sender, RoutedEventArgs e)
        {
            if (validarUpdate()) {
            
                // UPDATE

                int contador = 0;
                foreach (UIElement ui in this.horarios.Children){
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
                    if ( row > 0 && col < 2)
                        {

                            if (contador == 0)
                            {
                                listHorariosLlegadaUpdate.Add(textoCelda);
                                contador = 1;
                            }
                            else {
                                listHorariosSalidaUpdate.Add(textoCelda);
                                contador = 0;
                            }
                    }
                    

                }
                
            }

                
                MySqlConnection con;
                
                for (int i = 0; i < (paradas.Count); i++)
                {
                    con = conexionDB.ObtenerConexion();
                    Viaje ciudad = ViajeFacade.buscarPorId(this.id_viaje);
                    
                    Parada paradaCiudad = ParadaFacade.buscarPorRecorridoCiudad(ciudad.recorrido, this.paradas[i]);

                    string updateString = "UPDATE HORARIOS SET LLEGADA=?llegada, SALIDA=?salida WHERE VIAJE=?viaje AND PARADA=?parada";
                    MySqlCommand updateCommand = new MySqlCommand(updateString, con);
                    updateCommand.Parameters.Add("?llegada", listHorariosLlegadaUpdate[i]);
                    updateCommand.Parameters.Add("?salida", listHorariosSalidaUpdate[i]);
                    updateCommand.Parameters.Add("?viaje", this.id_viaje);
                    updateCommand.Parameters.Add("?parada", paradaCiudad.id);

                    updateCommand.ExecuteNonQuery();
                    con.Close();
                }

                MySqlConnection con2 = conexionDB.ObtenerConexion() ;
                string updateDatosViajes = "UPDATE VIAJES_DIARIOS SET BUS=?bus, AUXILIAR=?auxiliar,CHOFER=?chofer WHERE VIAJE=?viaje";
                MySqlCommand updateDatosCommand = new MySqlCommand(updateDatosViajes, con2);
                updateDatosCommand.Parameters.Add("?bus", comboBus.Text);
                updateDatosCommand.Parameters.Add("?auxiliar", obtenerRutPersonaByNombre(comboAuxiliar.Text, "AUXILIAR"));
                updateDatosCommand.Parameters.Add("?chofer", obtenerRutPersonaByNombre(comboChofer.Text, "CHOFER"));
                updateDatosCommand.Parameters.Add("?viaje", this.id_viaje);

                updateDatosCommand.ExecuteNonQuery();
                con2.Close();

                nuevoViajeDiario = new nuevoViaje();
                nuevoViajeDiario.Show();
                nuevoViajeDiario.saludo.Text = "Gracias por actualizar nuestros registros";
                nuevoViajeDiario.textBlock1.Text = "Se ha actualizado correctamente el viaje en el sistema";
            }
        }
        }
    }
    

