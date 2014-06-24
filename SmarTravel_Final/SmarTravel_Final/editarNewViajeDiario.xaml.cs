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
using SmarTravel_Final.Controller;
namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para editarNewViajeDiario.xaml
    /// </summary>
    public partial class editarNewViajeDiario : Window
    {
        int id_viaje = 0;
        public static nuevoViaje nuevoViajeDiario =null;
        public editarNewViajeDiario()
        {
            InitializeComponent();
        }
        public void getIdViaje(int viaje)
        {
            this.id_viaje = viaje;
        }

        public void crearTabla()
        {
            this.fechasViajes.RowDefinitions.Clear();
            TextBox celdas;
            try
            {
                // ENCABEZADO 3 COLUMNAS EN LA PRIMERA FILA

                this.fechasViajes.ColumnDefinitions.Add(new ColumnDefinition());
                this.fechasViajes.ColumnDefinitions.Add(new ColumnDefinition());
                this.fechasViajes.ColumnDefinitions.Add(new ColumnDefinition());
                this.fechasViajes.ColumnDefinitions.Add(new ColumnDefinition());
                this.fechasViajes.ColumnDefinitions.Add(new ColumnDefinition());
                this.fechasViajes.RowDefinitions.Add(new RowDefinition());


                Label FechaHeader = new Label();
                FechaHeader.Content = "FECHA VIAJE";
                FechaHeader.Style = Resources["HeaderTabla"] as Style;

                FechaHeader.SetValue(Grid.ColumnProperty, 0);
                FechaHeader.SetValue(Grid.RowProperty, 0);
                this.fechasViajes.Children.Add(FechaHeader);

                Label busHeader = new Label();
                busHeader.Content = "BUS";
                busHeader.Style = Resources["HeaderTabla"] as Style;

                busHeader.SetValue(Grid.ColumnProperty, 1);
                busHeader.SetValue(Grid.RowProperty, 0);
                this.fechasViajes.Children.Add(busHeader);

                Label choferHeader = new Label();
                choferHeader.Content = "CHOFER";
                choferHeader.Style = Resources["HeaderTabla"] as Style;
                choferHeader.Width = 200;
                choferHeader.SetValue(Grid.ColumnProperty, 2);
                choferHeader.SetValue(Grid.RowProperty, 0);
                this.fechasViajes.Children.Add(choferHeader);

                Label auxiliarHeader = new Label();
                auxiliarHeader.Content = "AUXILIAR";
                auxiliarHeader.Style = Resources["HeaderTabla"] as Style;
                auxiliarHeader.Width = 200;
                auxiliarHeader.SetValue(Grid.ColumnProperty, 3);
                auxiliarHeader.SetValue(Grid.RowProperty, 0);
                this.fechasViajes.Children.Add(auxiliarHeader);

                Label botonHeader = new Label();
                botonHeader.Content = "";
                botonHeader.Style = Resources["HeaderTabla"] as Style;
                botonHeader.Width = 200;
                botonHeader.SetValue(Grid.ColumnProperty, 4);
                botonHeader.SetValue(Grid.RowProperty, 0);
                this.fechasViajes.Children.Add(botonHeader);

                List<ViajeDiario> listFechasViajes = ViajeDiarioFacade.buscarViajeFecha(this.id_viaje);

                this.fechasViajes.ColumnDefinitions[0].Width = new System.Windows.GridLength(90);
                this.fechasViajes.ColumnDefinitions[1].Width = new System.Windows.GridLength(130);
                this.fechasViajes.ColumnDefinitions[2].Width = new System.Windows.GridLength(165);
                this.fechasViajes.ColumnDefinitions[3].Width = new System.Windows.GridLength(165);
                this.fechasViajes.ColumnDefinitions[4].Width = new System.Windows.GridLength(75);
                int largo = listFechasViajes.Count;
                for (int z = 0; z < largo; z++)
                {

                    this.fechasViajes.RowDefinitions.Add(new RowDefinition());
                    this.fechasViajes.RowDefinitions[z].Height = new System.Windows.GridLength(30);
                }
                this.fechasViajes.RowDefinitions[0].Height = new System.Windows.GridLength(30);
                this.fechasViajes.RowDefinitions[fechasViajes.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(30);

                int contador = 0;
                for (int row = 1; row < this.fechasViajes.RowDefinitions.Count; row++)
                {
                    for (int col = 0; col < this.fechasViajes.ColumnDefinitions.Count; col++)
                    {
                        if (col == 0)
                        {
                            celdas = new TextBox();
                            celdas.Style = Resources["ItemTablaGuion"] as Style;
                            celdas.Text = listFechasViajes[contador].fecha;
                            celdas.SetValue(Grid.ColumnProperty, col);
                            celdas.SetValue(Grid.RowProperty, row);
                            celdas.IsReadOnly = true;
                            this.fechasViajes.Children.Add(celdas);
                        }
                        if (col == 1)
                        {
                            celdas = new TextBox();
                            celdas.Style = Resources["ItemTablaGuion"] as Style;
                            celdas.Text = listFechasViajes[contador].bus;
                            celdas.SetValue(Grid.ColumnProperty, col);
                            celdas.SetValue(Grid.RowProperty, row);
                            celdas.IsReadOnly = true;
                            this.fechasViajes.Children.Add(celdas);
                        }
                        if (col ==2)
                        {
                            celdas = new TextBox();
                            celdas.Style = Resources["ItemTablaGuion"] as Style;
                            celdas.Text = obtenerNombrePersonaByRut(listFechasViajes[contador].chofer,"CHOFER");
                            celdas.SetValue(Grid.ColumnProperty, col);
                            celdas.SetValue(Grid.RowProperty, row);
                            celdas.IsReadOnly = true;
                            this.fechasViajes.Children.Add(celdas);
                        }
                        if (col == 3)
                        {
                            celdas = new TextBox();
                            celdas.Style = Resources["ItemTablaGuion"] as Style;
                            celdas.Text = obtenerNombrePersonaByRut(listFechasViajes[contador].auxiliar, "AUXILIAR") ;
                            celdas.SetValue(Grid.ColumnProperty, col);
                            celdas.SetValue(Grid.RowProperty, row);
                            celdas.IsReadOnly = true;
                            this.fechasViajes.Children.Add(celdas);
                        }
                        if (col == 4)
                        {
                            
                            Button editarViajeDiario = new Button();
                            editarViajeDiario.Click += new RoutedEventHandler(editarViajeDiario_Click);

                            editarViajeDiario.Content = "Editar";
               
                            editarViajeDiario.Tag = Convert.ToString(listFechasViajes[contador].fecha);
                            editarViajeDiario.SetValue(Grid.ColumnProperty, col);
                            editarViajeDiario.SetValue(Grid.RowProperty, row);
                            this.fechasViajes.Children.Add(editarViajeDiario);
                        }
                    }
                    contador++;
                }
                
            } // Fin TRY



            catch (Exception ex)
            {
                validar validarError = new validar();
                validarError.show(ex.Message);
            }
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
        public void editarViajeDiario_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var fila = button.Tag;

            ViajeDiario viajeBuscado = ViajeDiarioFacade.buscarViajeUnico(this.id_viaje,Convert.ToString(fila));
            this.comboBus.Text = viajeBuscado.bus;
            this.comboAuxiliar.Text = obtenerNombrePersonaByRut(viajeBuscado.auxiliar,"AUXILIAR");
            this.comboChofer.Text = obtenerNombrePersonaByRut(viajeBuscado.chofer,"CHOFER");
            this.fecha.Text = fila.ToString();
               

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
        private void editar_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection con = conexionDB.ObtenerConexion();
            string updateDatosViajes = "UPDATE VIAJES_DIARIOS SET BUS=?bus, AUXILIAR=?auxiliar,CHOFER=?chofer WHERE VIAJE=?viaje AND fecha=?fecha";
            MySqlCommand updateDatosCommand = new MySqlCommand(updateDatosViajes, con);
            updateDatosCommand.Parameters.Add("?bus", comboBus.Text);
            updateDatosCommand.Parameters.Add("?auxiliar", obtenerRutPersonaByNombre(comboAuxiliar.Text, "AUXILIAR"));
            updateDatosCommand.Parameters.Add("?chofer", obtenerRutPersonaByNombre(comboChofer.Text, "CHOFER"));
            updateDatosCommand.Parameters.Add("?viaje", this.id_viaje);
            updateDatosCommand.Parameters.Add("?fecha", fecha.Text);
            updateDatosCommand.ExecuteNonQuery();
            con.Close();

            nuevoViajeDiario = new nuevoViaje();
            nuevoViajeDiario.Show();
            nuevoViajeDiario.saludo.Text = "Gracias por actualizar un viaje en la fecha: "+fecha.Text;
            nuevoViajeDiario.textBlock1.Text = "Se ha actualizado correctamente el viaje diario";

            crearTabla();


        }
    }
}
