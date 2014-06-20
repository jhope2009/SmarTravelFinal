using MySql.Data.MySqlClient;
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
    /// Lógica de interacción para ventaPasaje.xaml
    /// </summary>
    public partial class ventaPasaje : Window
    {
        private int idViaje;
        private int idDiario;

        public ventaPasaje(int idViaje, int idDiario)
        {
            this.idViaje = idViaje;
            this.idDiario = idDiario;
            InitializeComponent();
        }                

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            Viaje viaje = ViajeFacade.buscarPorId(this.idViaje);
            ViajeDiario viajeDiario = new ViajeDiario();
            foreach (ViajeDiario vd in viaje.viajesDiarios)
            {
                if (vd.id == idDiario) viajeDiario = vd;
            }
            foreach (Horario h in viaje.horarios)
            {
                if (h.parada.ciudad.nombre == viajeDiario.trayecto.origen.nombre) this.textoSalida.Text = h.salida;
                if (h.parada.ciudad.nombre == viajeDiario.trayecto.destino.nombre) this.textoLlegada.Text = h.llegada;
            }
           
            if (viajeDiario.id > -1)
            {                
                int precio = TrayectoFacade.buscarPorId(viajeDiario.trayecto.id).precio;
                
                this.textoFecha.Text = viajeDiario.fecha;
                this.textoOrigen.Text = viajeDiario.trayecto.origen.nombre;
                this.textoDestino.Text = viajeDiario.trayecto.destino.nombre;
                
                this.textoTarifa.Text = precio.ToString();
                this.textoTotal.Text = precio.ToString();
            }
        }

        private void botonCliente_Click(object sender, RoutedEventArgs e)
        {
            this.botonCliente.IsEnabled = false;
            this.botonClienteCancelar.Visibility = Visibility.Visible;
        }

        private void botonClienteCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.botonClienteCancelar.Visibility = Visibility.Hidden;
            this.gridDatosCliente.Visibility = Visibility.Hidden;
            this.textoNombre.Clear();
            this.textoPorcentaje.Clear();
            this.textoCliente.Clear();
            this.textoDescuento.Text = "0";
            this.textoTotal.Text = this.textoTarifa.Text;
            this.botonCliente.IsEnabled = true;
        }

        private void textoCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.textoCliente.Text != "")
                {
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    try
                    {                        
                        string sql = "select p.NOMBRE_COMPLETO, c.DESCUENTO from persona as p inner join cliente as c on(p.RUT=c.RUT) WHERE p.RUT = '" + this.textoCliente.Text.ToString() + "'";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        MySqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {                                
                                this.textoNombre.Text = dr.GetString(0);
                                this.textoPorcentaje.Text = dr.GetInt32(1).ToString() + " %";
                                int precio = Convert.ToInt32(this.textoTarifa.Text);
                                decimal porcent = ((decimal)dr.GetInt32(1)) / 100;
                                int dscto = (int)(precio * porcent);
                                precio = precio - dscto;
                                this.textoDescuento.Text = dscto.ToString();
                                this.textoTotal.Text = precio.ToString();
                            }
                            dr.Close();
                            this.gridDatosCliente.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            this.textoCliente.Clear();
                            this.textoCliente.Focus();
                            this.textoNombre.Clear();
                            this.textoPorcentaje.Clear();
                            this.gridDatosCliente.Visibility = Visibility.Hidden;
                            validar alert = new validar();
                            alert.show("El cliente no se encuentra registrado. ");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.textoCliente.Clear();
                        this.textoCliente.Focus();
                        this.textoNombre.Clear();
                        this.textoPorcentaje.Clear();
                        this.gridDatosCliente.Visibility = Visibility.Hidden;
                        Console.WriteLine(ex.Message);
                        validar alert = new validar();
                        alert.show("Error al obtener los datos. ");
                    }
                    finally
                    {
                        con.Close();
                    }                    
                }
                else
                {
                    this.textoCliente.Clear();
                    this.textoCliente.Focus();
                    this.textoNombre.Clear();
                    this.textoPorcentaje.Clear();
                    this.gridDatosCliente.Visibility = Visibility.Hidden;
                    validar alert = new validar();
                    alert.show("Ingrese el RUT del cliente ");
                }
            }
        }

        private void botonParadas_Click(object sender, RoutedEventArgs e)
        {
            paradasViaje pv = new paradasViaje(this.idViaje,this.textoOrigen.Text.ToString(), this.textoDestino.Text.ToString());
            pv.Show();
        }                
    }
}
