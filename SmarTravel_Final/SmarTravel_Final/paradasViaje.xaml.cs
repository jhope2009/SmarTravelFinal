using SmarTravel_Final.Controller;
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

namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para paradasViaje.xaml
    /// </summary>
    public partial class paradasViaje : Window
    {
        public paradasViaje(int idViaje, string origen, string destino)
        {
            InitializeComponent();
            Viaje viaje = ViajeFacade.buscarPorId(idViaje);
            if (viaje != null)
            {
                Label valor;
                string[] itemsTabla = new string[3];
                bool intermedio = false;
                foreach(Horario h in viaje.horarios)
                {
                    if(intermedio == false)
                    {
                        if(h.parada.ciudad.nombre == origen)
                        {
                            intermedio = true;
                            itemsTabla[0] = h.parada.ciudad.nombre;
                            itemsTabla[1] = h.salida;
                            itemsTabla[2] = h.llegada;

                            this.tablaParadas.RowDefinitions.Add(new RowDefinition());
                            this.tablaParadas.RowDefinitions[this.tablaParadas.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(30);
                            for (int n = 0; n < itemsTabla.Length; n++)
                            {
                                valor = new Label();
                                valor.Style = Resources["ItemViaje"] as Style;
                                valor.Content = itemsTabla[n];
                                valor.HorizontalAlignment = HorizontalAlignment.Center;
                                valor.SetValue(Grid.ColumnProperty, n);
                                valor.SetValue(Grid.RowProperty, this.tablaParadas.RowDefinitions.Count - 1);
                                this.tablaParadas.Children.Add(valor);
                            }
                        }
                    }
                    else
                    {
                        if(h.parada.ciudad.nombre == destino)
                        {
                            itemsTabla[0] = h.parada.ciudad.nombre;
                            itemsTabla[1] = h.salida;
                            itemsTabla[2] = h.llegada;

                            this.tablaParadas.RowDefinitions.Add(new RowDefinition());
                            this.tablaParadas.RowDefinitions[this.tablaParadas.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(30);
                            for (int n = 0; n < itemsTabla.Length; n++)
                            {
                                valor = new Label();
                                valor.Style = Resources["ItemViaje"] as Style;
                                valor.Content = itemsTabla[n];
                                valor.HorizontalAlignment = HorizontalAlignment.Center;
                                valor.SetValue(Grid.ColumnProperty, n);
                                valor.SetValue(Grid.RowProperty, this.tablaParadas.RowDefinitions.Count - 1);
                                this.tablaParadas.Children.Add(valor);
                            }
                            break;
                        }
                        else
                        {
                            itemsTabla[0] = h.parada.ciudad.nombre;
                            itemsTabla[1] = h.salida;
                            itemsTabla[2] = h.llegada;

                            this.tablaParadas.RowDefinitions.Add(new RowDefinition());
                            this.tablaParadas.RowDefinitions[this.tablaParadas.RowDefinitions.Count - 1].Height = new System.Windows.GridLength(30);
                            for (int n = 0; n < itemsTabla.Length; n++)
                            {
                                valor = new Label();
                                valor.Style = Resources["ItemViaje"] as Style;
                                valor.Content = itemsTabla[n];
                                valor.HorizontalAlignment = HorizontalAlignment.Center;
                                valor.SetValue(Grid.ColumnProperty, n);
                                valor.SetValue(Grid.RowProperty, this.tablaParadas.RowDefinitions.Count - 1);
                                this.tablaParadas.Children.Add(valor);
                            }
                        }
                    }                    
                }
            }            
        }

        private void botonBuscar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
