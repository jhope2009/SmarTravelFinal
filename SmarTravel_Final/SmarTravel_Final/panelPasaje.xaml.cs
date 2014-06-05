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
using SmarTravel_Final.Controller;


namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para panelPasaje.xaml
    /// </summary>
    public partial class panelPasaje : UserControl
    {
        public panelPasaje()
        {
            InitializeComponent();
        }
        
        private void botonBuscar_Click(object sender, RoutedEventArgs e)
        {
            this.gridMuestraViajes.Visibility = Visibility.Hidden;

            this.tablaViajes.Children.Clear();
            this.tablaViajes.RowDefinitions.Clear();
            if (this.comboOrigen.SelectedIndex > -1 && this.comboDestino.SelectedIndex > -1)
            {
                if (this.buscarFecha.Text != "")
                {
                    Label valor;
                    List<string> precio = new List<string>();
                    List<int> recorrido = new List<int>();
                    List<Trayecto> trayectos = new List<Trayecto>();
                    //List<int> trayectos = new List<int>();
                    List<int> viaje = new List<int>();
                    string[] l = new string[4];
                    List<string> identificador = new List<string>();
                    string salida = "";
                    string llegada = "";
                    string hora = "";

                    string tramo = (string)this.comboOrigen.SelectedItem + "-" + (string)this.comboDestino.SelectedItem;
                    string fecha = this.buscarFecha.Text;
                    Ciudad origen = CiudadFacade.buscarPorNombre(this.comboOrigen.SelectedItem.ToString());
                    Ciudad destino = CiudadFacade.buscarPorNombre(this.comboDestino.SelectedItem.ToString());
                    /*
                    try
                    {                        
                        trayectos = TrayectoFacade.bus
                        
                        
                        sql = "select t.ID, t.RECORRIDO, t.PRECIO from trayecto as t where t.ORIGEN=" + origen + " and t.DESTINO=" + destino;
                        cmd = new MySqlCommand(string.Format(sql), con);
                        MySqlDataReader dr1 = cmd.ExecuteReader();
                        while (dr1.Read())
                        {
                            precio.Add((string)dr1.GetInt32(2).ToString());
                            recorrido.Add(dr1.GetInt32(1));
                            trayecto.Add(dr1.GetInt32(0));
                        }
                        dr1.Close();
                        
                        for (int i = 0; i < precio.Count; i++)
                        {
                            sql = "select vd.VIAJE, v.IDENTIFICADOR, vd.ASIENTOS_DISPONIBLES from viajes as v inner join viajes_diarios as vd on (v.ID=vd.VIAJE) where v.RECORRIDO = " + recorrido[i] + " and vd.FECHA='" + fecha + "' and vd.TRAYECTO=" + trayectos[i];
                            cmd = new MySqlCommand(string.Format(sql), con);
                            MySqlDataReader dr2 = cmd.ExecuteReader();
                            while (dr2.Read())
                            {
                                viaje.Add(dr2.GetInt32(0));
                                identificador.Add(dr2.GetString(1));
                            }
                            dr2.Close();
                            for (int j = 0; j < viaje.Count; j++)
                            {
                                sql = "select h.SALIDA from horarios as h inner join parada as p on (h.PARADA=p.ID) where h.VIAJE=" + viaje[j] + " and p.CIUDAD=" + origen;
                                cmd = new MySqlCommand(string.Format(sql), con);
                                MySqlDataReader dr3 = cmd.ExecuteReader();
                                while (dr3.Read())
                                {
                                    salida = dr3.GetString(0);
                                }
                                dr3.Close();

                                sql = "select h.LLEGADA from horarios as h inner join parada as p on (h.PARADA=p.ID) where h.VIAJE=" + viaje[j] + " and p.CIUDAD=" + destino;
                                cmd = new MySqlCommand(string.Format(sql), con);
                                MySqlDataReader dr4 = cmd.ExecuteReader();
                                while (dr4.Read())
                                {
                                    llegada = dr4.GetString(0);
                                }
                                dr4.Close();

                                hora = salida + " - " + llegada;
                                l[0] = tramo;
                                l[1] = hora;
                                l[2] = precio[i];
                                l[3] = identificador[j];

                                this.tablaViajes.RowDefinitions.Add(new RowDefinition());
                                this.tablaViajes.RowDefinitions[this.tablaViajes.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(30);
                                for (int n = 0; n < l.Length; n++)
                                {
                                    valor = new Label();
                                    valor.Style = Resources["ItemViaje"] as Style;
                                    valor.Content = l[n];
                                    valor.SetValue(Grid.ColumnProperty, n);
                                    valor.SetValue(Grid.RowProperty, this.tablaViajes.RowDefinitions.Count - 1);
                                    this.tablaViajes.Children.Add(valor);
                                }
                            }
                        }
                        this.gridMuestraViajes.Visibility = Visibility.Visible;
                    }
                    catch (Exception ex)
                    {
                        validar alert = new validar();
                        alert.show(ex.Message);
                    }
            */
                }
                else
                {
                    validar alert = new validar();
                    alert.show("Seleccione una fecha de busqueda.");
                }
            }
            else
            {
                validar alert = new validar();
                alert.show("Debe seleccionar origen y destino.");
            }
        }

        private void comboOrigen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboOrigen.SelectedIndex != -1)
            {
                List<Trayecto> destinos = TrayectoFacade.buscarDestinosPorOrigen(this.comboOrigen.SelectedItem.ToString());
                this.comboDestino.Items.Clear();
                foreach (Trayecto d in destinos)
                {
                    this.comboDestino.Items.Add(d.destino.nombre);
                }
            }
        }

        private void tabReservaViaje_Selected(object sender, RoutedEventArgs e)
        {
            this.comboOrigen.Items.Clear();
            this.comboDestino.Items.Clear();

            this.comboOrigen.Items.Clear();
            List<Trayecto> origenes = TrayectoFacade.buscarOrigenes();
            foreach (Trayecto t in origenes)
            {
                this.comboOrigen.Items.Add(t.origen.nombre);
            }
        }
    }
}
