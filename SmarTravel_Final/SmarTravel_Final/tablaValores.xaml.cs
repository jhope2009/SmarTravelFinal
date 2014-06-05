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
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using SmarTravel_Final.Controller;

namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para tablaValores.xaml
    /// </summary>
    public partial class tablaValores : Window
    {
        public Grid valores
        {
            get { return this.tabla; }
        }
        private List<string> lp;
        private Dictionary<string, int> ciudades = new Dictionary<string, int>();


        public tablaValores(List<string> lp, Dictionary<string, int> ciudades)
        {
            InitializeComponent();
            this.lp = lp;
            this.ciudades = ciudades;
            TextBox text;
            Label head;
            this.tabla.ColumnDefinitions.Add(new ColumnDefinition());
            this.tabla.RowDefinitions.Add(new RowDefinition());
            head = new Label();
            head.Style = Resources["HeaderTablaPrecios"] as Style;
            head.Content = " ";
            head.SetValue(Grid.ColumnProperty, 0);
            head.SetValue(Grid.RowProperty, 0);
            this.tabla.Children.Add(head);
            this.tabla.ColumnDefinitions[0].Width = new System.Windows.GridLength(90);
            this.tabla.RowDefinitions[0].Height = new System.Windows.GridLength(30);

            for (int i = 1; i <= lp.Count; i++)
            {
                this.tabla.ColumnDefinitions.Add(new ColumnDefinition());
                this.tabla.RowDefinitions.Add(new RowDefinition());
                this.tabla.ColumnDefinitions[i].Width = new System.Windows.GridLength(80);
                this.tabla.RowDefinitions[i].Height = new System.Windows.GridLength(30);
                //Header fila
                head = new Label();
                head.Style = Resources["HeaderTablaPrecios"] as Style;
                head.Content = lp[i - 1];
                head.SetValue(Grid.ColumnProperty, 0);
                head.SetValue(Grid.RowProperty, i);
                this.tabla.Children.Add(head);
                //Header columna
                head = new Label();
                head.Style = Resources["HeaderTablaPrecios"] as Style;
                head.Content = lp[i - 1];
                head.SetValue(Grid.ColumnProperty, i);
                head.SetValue(Grid.RowProperty, 0);
                this.tabla.Children.Add(head);
            }

            for (int row = 1; row < this.tabla.RowDefinitions.Count; row++)
            {
                for (int col = 1; col < this.tabla.ColumnDefinitions.Count; col++)
                {
                    if (col != row)
                    {
                        if (row > col)
                        {
                            text = new TextBox();
                            text.IsReadOnly = true;
                            text.Style = Resources["NullTabla"] as Style;
                            text.Text = " ";
                            text.SetValue(Grid.ColumnProperty, col);
                            text.SetValue(Grid.RowProperty, row);
                            this.tabla.Children.Add(text);
                        }
                        else
                        {
                            text = new TextBox();
                            text.KeyDown += new KeyEventHandler(text_KeyDown);
                            text.Style = Resources["ItemTabla"] as Style;
                            text.Text = "0";
                            text.SetValue(Grid.ColumnProperty, col);
                            text.SetValue(Grid.RowProperty, row);
                            this.tabla.Children.Add(text);
                        }
                    }
                    else
                    {
                        text = new TextBox();
                        text.IsReadOnly = true;
                        text.Style = Resources["NullTabla"] as Style;
                        text.Text = " ";
                        text.SetValue(Grid.ColumnProperty, col);
                        text.SetValue(Grid.RowProperty, row);
                        this.tabla.Children.Add(text);
                    }
                }
            }
        }

        public tablaValores(List<string> lp, Dictionary<string, int> ciudades, string[,] precios)
        {
            InitializeComponent();
            this.Guardar.Visibility = Visibility.Hidden;
            this.lp = lp;
            this.ciudades = ciudades;
            TextBox text;
            Label head;
            this.tabla.ColumnDefinitions.Add(new ColumnDefinition());
            this.tabla.RowDefinitions.Add(new RowDefinition());
            head = new Label();
            head.Style = Resources["HeaderTablaPrecios"] as Style;
            head.Content = " ";
            head.SetValue(Grid.ColumnProperty, 0);
            head.SetValue(Grid.RowProperty, 0);
            this.tabla.Children.Add(head);
            this.tabla.ColumnDefinitions[0].Width = new System.Windows.GridLength(90);
            this.tabla.RowDefinitions[0].Height = new System.Windows.GridLength(30);

            for (int i = 1; i <= lp.Count; i++)
            {
                this.tabla.ColumnDefinitions.Add(new ColumnDefinition());
                this.tabla.RowDefinitions.Add(new RowDefinition());
                this.tabla.ColumnDefinitions[i].Width = new System.Windows.GridLength(80);
                this.tabla.RowDefinitions[i].Height = new System.Windows.GridLength(30);
                //Header fila
                head = new Label();
                head.Style = Resources["HeaderTablaPrecios"] as Style;
                head.Content = lp[i - 1];
                head.SetValue(Grid.ColumnProperty, 0);
                head.SetValue(Grid.RowProperty, i);
                this.tabla.Children.Add(head);
                //Header columna
                head = new Label();
                head.Style = Resources["HeaderTablaPrecios"] as Style;
                head.Content = lp[i - 1];
                head.SetValue(Grid.ColumnProperty, i);
                head.SetValue(Grid.RowProperty, 0);
                this.tabla.Children.Add(head);
            }

            for (int row = 1; row < this.tabla.RowDefinitions.Count; row++)
            {
                for (int col = 1; col < this.tabla.ColumnDefinitions.Count; col++)
                {
                    if (col != row)
                    {
                        if (row > col)
                        {
                            text = new TextBox();
                            text.IsReadOnly = true;
                            text.Style = Resources["NullTabla"] as Style;
                            text.Text = " ";
                            text.SetValue(Grid.ColumnProperty, col);
                            text.SetValue(Grid.RowProperty, row);
                            this.tabla.Children.Add(text);
                        }
                        else
                        {
                            text = new TextBox();
                            text.IsReadOnly = true;
                            text.Style = Resources["ItemTabla"] as Style;
                            text.Text = precios[row - 1, col - 1];
                            text.SetValue(Grid.ColumnProperty, col);
                            text.SetValue(Grid.RowProperty, row);
                            this.tabla.Children.Add(text);
                        }
                    }
                    else
                    {
                        text = new TextBox();
                        text.IsReadOnly = true;
                        text.Style = Resources["NullTabla"] as Style;
                        text.Text = " ";
                        text.SetValue(Grid.ColumnProperty, col);
                        text.SetValue(Grid.RowProperty, row);
                        this.tabla.Children.Add(text);
                    }
                }
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (this.lp.Count > 1)
            {
                TextBox txt;                
                int idParada = -1;
                List<Trayecto> trayectos = new List<Trayecto>();
                string[,] precios = new string[this.lp.Count, this.lp.Count];

                foreach (UIElement ui in this.tabla.Children)
                {
                    int row = System.Windows.Controls.Grid.GetRow(ui);
                    int col = System.Windows.Controls.Grid.GetColumn(ui);

                    if (row != 0 && col != 0)
                    {
                        txt = (TextBox)ui;
                        precios[row - 1, col - 1] = txt.Text;
                    }
                }
                try
                {
                    idParada = ParadaFacade.obtenerProximoId();
                    idParada = idParada + lp.Count-1;
                    Parada parada = new Parada(idParada, CiudadFacade.buscarPorNombre(lp[lp.Count-1]), new Parada());
                    Parada siguiente = parada;
                    Console.WriteLine("Ultima parada: "+parada.id+" siguiente: "+parada.siguiente.id);

                    //     Estructura las paradas
                    for (int i = lp.Count - 2; i > -1; i--)
                    {
                        idParada = idParada - 1;
                        parada = new Parada(idParada, CiudadFacade.buscarPorNombre(lp[i]), siguiente);
                        siguiente = parada;
                        Console.WriteLine("Parada: " + parada.id+" siguiente: "+parada.siguiente.id);
                    }
                    // estructura los trayectos
                    for (int i = 0; i < lp.Count; i++)
                    {
                        for (int j = i + 1; j < lp.Count; j++)
                        {
                            trayectos.Add(new Trayecto(CiudadFacade.buscarPorNombre(lp[i]), CiudadFacade.buscarPorNombre(lp[j]), Convert.ToInt32(precios[i, j])));
                        }
                    }
                    Recorrido recorrido = new Recorrido(parada, trayectos);

                    try
                    {
                        RecorridoFacade.guardar(recorrido);
                        okAlerta alert = new okAlerta();
                        alert.show("Recorrido Ingresado Correctamente");
                        this.DialogResult = true;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        validar alert = new validar();
                        alert.show("No se pudo Guardar el Recorrido. Por favor verifique los datos");
                    }
                }
                catch (Exception ex)
                {
                    validar alert = new validar();
                    alert.show("Error al obtener datos. Verifique las paradas nuevamente.");
                }
            }
            else
            {
                validar alert = new validar();
                alert.show("El Recorrido debe tener como minimo dos Paradas");
            }
        }

        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void tabla_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

    }
}
