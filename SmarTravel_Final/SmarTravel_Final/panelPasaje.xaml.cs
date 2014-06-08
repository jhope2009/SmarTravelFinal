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
                    bool existen = false;
                    Label valor;
                    Button ver;
                    string[] itemsTabla = new string[4];
                    string salida = "";
                    string llegada = "";
                    string fecha = this.buscarFecha.Text;

                    Ciudad origen = CiudadFacade.buscarPorNombre(this.comboOrigen.SelectedItem.ToString());
                    Ciudad destino = CiudadFacade.buscarPorNombre(this.comboDestino.SelectedItem.ToString());

                    try
                    {
                        List<Viaje> viajes = ViajeFacade.buscarPorOrigenDestino(origen.nombre, destino.nombre);
                        Console.WriteLine(viajes.Count);
                        if (viajes.Count > 0)
                        {
                            foreach (Viaje viaje in viajes)
                            {                                
                                foreach (ViajeDiario vd in viaje.viajesDiarios)
                                {
                                    if (vd.fecha == fecha && vd.trayecto.origen.id == origen.id && vd.trayecto.destino.id == destino.id)
                                    {
                                        existen = true;
                                        foreach (Horario horario in viaje.horarios)
                                        {
                                            if (horario.parada.ciudad.id == origen.id) salida = origen.nombre + " - " + horario.salida;
                                            if (horario.parada.ciudad.id == destino.id) llegada = destino.nombre + " - " + horario.llegada;
                                        }                                      

                                        itemsTabla[0] = salida;
                                        itemsTabla[1] = llegada;
                                        itemsTabla[2] = vd.trayecto.precio.ToString();
                                        itemsTabla[3] = viaje.identificador;
                                        
                                        this.tablaViajes.RowDefinitions.Add(new RowDefinition());
                                        this.tablaViajes.RowDefinitions[this.tablaViajes.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(30);
                                        for (int n = 0; n < itemsTabla.Length; n++)
                                        {
                                            valor = new Label();
                                            valor.Style = Resources["ItemViaje"] as Style;
                                            valor.Content = itemsTabla[n];
                                            valor.HorizontalAlignment = HorizontalAlignment.Center;
                                            valor.SetValue(Grid.ColumnProperty, n);
                                            valor.SetValue(Grid.RowProperty, this.tablaViajes.RowDefinitions.Count - 1);
                                            this.tablaViajes.Children.Add(valor);
                                        }
                                        BitmapImage btm = new BitmapImage(new Uri("/SmarTravel_Final;component/Images/continue.png", UriKind.Relative));
                                        Image img = new Image();
                                        img.Source = btm;
                                        img.Stretch = Stretch.Uniform;
                                        
                                        ver = new Button();
                                        ver.Cursor = Cursors.Hand;
                                        ver.Style = Resources["BotonSinBorde"] as Style;
                                        ver.Content = img;
                                        ver.Click += new RoutedEventHandler(verViaje_Click);
                                        ver.Tag = vd.id.ToString();
                                        ver.SetValue(Grid.ColumnProperty, 4);
                                        ver.SetValue(Grid.RowProperty, this.tablaViajes.RowDefinitions.Count - 1);
                                        this.tablaViajes.Children.Add(ver);
                                    }
                                }                               
                            }
                            if(existen == true) this.gridMuestraViajes.Visibility = Visibility.Visible;
                            else
                            {
                                validar alert = new validar();
                                alert.show("No existen viajes disponibles para esta fecha.");
                            }
                        }
                        else
                        {
                            validar alert = new validar();
                            alert.show("No existen viajes disponibles.");
                        }
                    }
                    catch (Exception ex)
                    {
                        validar alert = new validar();
                        alert.show("No se pudo obtener los viajes disponibles.");
                    }
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

        private void verViaje_Click(object sender, RoutedEventArgs e)
        {
            okAlerta alert = new okAlerta();
            alert.show("Viendo viaje...");
        }
    }
}
