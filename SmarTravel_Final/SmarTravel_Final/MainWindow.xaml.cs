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
using System.Data.Sql;
using System.IO;
using System.Data;
using System.Windows.Media.Animation;

namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string rutSession = "";
        panelUsuario panelUsuario;
        panelBuses panelBuses;
        panelRecorrido panelRecorrido;
        panelPasaje panelPasaje;
        public MainWindow()
        {
            InitializeComponent();

            this.usuario.IsEnabled = false;
            this.bus.IsEnabled = false;
            this.mapa.IsEnabled = false;
            this.encomienda.IsEnabled = false;
            this.registo.IsEnabled = false;
            //this.general.Children.Add(panelUsuario);
            //panelUsuario.Visibility = Visibility.Hidden;
            

        }
        // Click en un usuario
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)52, (byte)25, (byte)84));
            if (this.usuario.Background == Brushes.White)
            {
                this.usuario.Background = colorFondo;
                /*this.usuario.Icon = new System.Windows.Controls.Image
                {
                   Source = new BitmapImage(new Uri("/SmarTravel_Final;component/Images/usuario.png", UriKind.Relative))
                };*/
            }
            else
            {
                /*this.usuario.Icon = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri("/SmarTravel;component/Images/usuario_hover.png", UriKind.Relative))
                };
                //this.usuario.Width = 100;
                //this.usuario.Height = 100;
                //this.usuario.st
        */

                this.usuario.Background = Brushes.White;
                this.bus.Background = colorFondo;
                this.mapa.Background = colorFondo;
                this.encomienda.Background = colorFondo;
                this.registo.Background = colorFondo;
            }

            panelUsuario.Visibility = Visibility.Visible;
            panelUsuario.general.Visibility = Visibility.Visible;
            panelUsuario.accionesUsuario.Visibility = Visibility.Visible;

            panelUsuario.listadoTabla.Visibility = Visibility.Hidden;
            panelUsuario.listadoUsuarios.Visibility = Visibility.Hidden;
            panelUsuario.busquedaUser.Text = "";
            
            panelUsuario.cbCargo.IsChecked = false;
            panelUsuario.cbNombre.IsChecked = false;
            panelUsuario.cbRut.IsChecked = false;

            panelUsuario.nombreCompleto.Text = "";
            panelUsuario.edad.Text = "";
            panelUsuario.rut.Text = "";
            panelUsuario.verificador.Text = "";
            panelUsuario.direccion.Text = "";
            panelUsuario.fono.Text = "";
            panelUsuario.clave.Text = "";
            panelUsuario.comboSexo.Text = "";
            panelUsuario.comboCiudad.Text = "";
            panelUsuario.comboCiudad.Items.Clear();
            panelUsuario.comboRegion.Text = "";
            panelUsuario.comboCargo.Text = "";

            this.panelBuses.Visibility = Visibility.Hidden;
            panelPasaje.Visibility = Visibility.Hidden;
            efectoPanel(panelUsuario);
        }
        private void bus_Click(object sender, RoutedEventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)52, (byte)25, (byte)84));
            if (this.bus.Background == Brushes.White)
                this.bus.Background = colorFondo;

            else
            {
                this.bus.Background = Brushes.White;
                this.usuario.Background = colorFondo;
                this.mapa.Background = colorFondo;
                this.encomienda.Background = colorFondo;
                this.registo.Background = colorFondo;
            }


            this.panelBuses.Visibility = Visibility.Visible;
            this.panelUsuario.Visibility = Visibility.Hidden;
            this.panelRecorrido.Visibility = Visibility.Hidden;
            this.contenido.Visibility = Visibility.Visible;
            efectoPanel(panelBuses);
            
        }
        private void cerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            this.inicio.Visibility = Visibility.Visible;
            this.rutUsuario.Text = "";
            this.passUsuario.Password = "";
            this.usuario.IsEnabled = false;
            this.bus.IsEnabled = false;
            this.mapa.IsEnabled = false;
            this.encomienda.IsEnabled = false;
            this.registo.IsEnabled = false;
            this.usuarioActual.Visibility = Visibility.Hidden;
            this.cerrarSesion.Visibility = Visibility.Hidden;

            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)52, (byte)25, (byte)84));
            this.bus.Background = colorFondo;
            this.usuario.Background = colorFondo;
            this.mapa.Background = colorFondo;
            this.encomienda.Background = colorFondo;
            this.registo.Background = colorFondo;
            rutSession = "";

            panelUsuario.Visibility = Visibility.Hidden;
            panelBuses.Visibility = Visibility.Hidden;
            panelRecorrido.Visibility = Visibility.Hidden;
            panelPasaje.Visibility = Visibility.Hidden;
        }
        private void mapa_Click(object sender, RoutedEventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)52, (byte)25, (byte)84));
            if (this.mapa.Background == Brushes.White)
                this.mapa.Background = colorFondo;


            else
            {
                this.mapa.Background = Brushes.White;
                this.usuario.Background = colorFondo;
                this.bus.Background = colorFondo;
                this.encomienda.Background = colorFondo;
                this.registo.Background = colorFondo;
            }

            panelUsuario.Visibility = Visibility.Hidden;
            panelRecorrido.Visibility = Visibility.Visible;
            panelBuses.Visibility = Visibility.Hidden;
            this.contenido.Visibility = Visibility.Visible;
            panelPasaje.Visibility = Visibility.Hidden;

            efectoPanel(panelRecorrido);
            //this.busquedaUsuario.Visibility = Visibility.Hidden;
            //this.accionesUsuario.Visibility = Visibility.Hidden;
            //ESTA NO
            //this.general.Children.Add(panelreco);
            //this.panelreco.Visibility = Visibility.Visible;
        }
        private void encomienda_Click(object sender, RoutedEventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)52, (byte)25, (byte)84));
            if (this.encomienda.Background == Brushes.White)
                this.encomienda.Background = colorFondo;

            else
            {
                this.encomienda.Background = Brushes.White;
                this.usuario.Background = colorFondo;
                this.bus.Background = colorFondo;
                this.mapa.Background = colorFondo;
                this.registo.Background = colorFondo;
            }
            panelPasaje.Visibility = Visibility.Hidden;
        }
        private void registo_Click(object sender, RoutedEventArgs e)
        {
            var colorFondo = new SolidColorBrush(Color.FromArgb(255, (byte)52, (byte)25, (byte)84));
            if (this.registo.Background == Brushes.White)
                this.registo.Background = colorFondo;

            else
            {
                this.registo.Background = Brushes.White;
                this.usuario.Background = colorFondo;
                this.bus.Background = colorFondo;
                this.mapa.Background = colorFondo;
                this.encomienda.Background = colorFondo;
            }
            panelUsuario.Visibility = Visibility.Hidden;
            panelBuses.Visibility = Visibility.Hidden;
            panelRecorrido.Visibility = Visibility.Hidden;
            panelPasaje.Visibility = Visibility.Visible;
            //panelPasaje.comboOrigen.SelectedIndex = -1;
            //panelPasaje.comboDestino.SelectedIndex = -1;
            this.contenido.Visibility = Visibility.Visible;
            panelPasaje.gridMuestraViajes.Visibility = Visibility.Hidden;
            efectoPanel(panelPasaje);
        }
        private void usuarioActual_Click(object sender, RoutedEventArgs e)
        {
            fichaPersonal ficha = new fichaPersonal();
            ficha.llenarFicha(rutSession);
            ficha.ShowDialog();
        }
        private Boolean validarLogin()
        {
            string rut = rutUsuario.Text;
            string pass = passUsuario.Password;

            if (rut == "")
            {
                validar mensajeValidacion = new validar();
                mensajeValidacion.show("Ingrese el número de rut para iniciar sesión.");
                return false;
            }
            else if (pass == "")
            {
                validar mensajeValidacion = new validar();
                mensajeValidacion.show("Ingrese la contraseña del usuario.");
                return false;
            }
            return true;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Boolean validar = validarLogin();

            if (validar)
            {

                MySqlDataReader dr;
                try
                {
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    //string rutUser = rutUsuario.Text;
                    //string pass = passUsuario.Password;
                    string rutUser = "18285166-3";
                    string pass = "FELIPE";
                    string sql = "SELECT RUT,CLAVE,NOMBRE_COMPLETO,CARGO FROM PERSONA WHERE RUT = '" + rutUser + "' AND CLAVE COLLATE latin1_bin = '" + pass + "' AND CARGO = 'ADMINISTRADOR'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        panelUsuario = new panelUsuario();
                        this.general.Children.Add(panelUsuario);
                        panelUsuario.Visibility = Visibility.Visible;
                        efectoPanel(panelUsuario);

                        panelBuses = new panelBuses();
                        this.contenido.Children.Add(panelBuses);
                        panelBuses.Visibility = Visibility.Hidden;

                        panelRecorrido = new panelRecorrido();
                        this.contenido.Children.Add(panelRecorrido);
                        panelRecorrido.Visibility = Visibility.Hidden;

                        panelPasaje = new panelPasaje();
                        this.contenido.Children.Add(panelPasaje);
                        panelPasaje.Visibility = Visibility.Hidden;
                        
                        while (dr.Read())
                        {
                            rutSession = rutUser;

                            string nombreUsuario = dr.GetValue(2).ToString();
                            string cargo = dr.GetValue(3).ToString();
                            // MessageBox.Show("Bienvenido " + nombreUsuario);
                            this.inicio.Visibility = Visibility.Hidden;
                            usuarioActual.Header = nombreUsuario.ToUpper() + " / " + cargo;
                            usuarioActual.Visibility = Visibility.Visible;
                            cerrarSesion.Visibility = Visibility.Visible;

                            this.usuario.IsEnabled = true;
                            this.bus.IsEnabled = true;
                            this.mapa.IsEnabled = true;
                            this.encomienda.IsEnabled = true;
                            this.registo.IsEnabled = true;
                            //this.panelUsuario.Visibility = Visibility.Visible;
                            //this.busquedaUsuario.Visibility = Visibility.Visible;

                            //this.busquedaUser.Text = "";
                            alerta mensaje = new alerta();
                            mensaje.show(nombreUsuario);
                            usuario.Background = Brushes.White;
                            //this.accionesUsuario.Visibility = Visibility.Visible;
                            

                        }
                        
                        

                    }
                    else
                    {

                        validar mensajeValidacion = new validar();
                        mensajeValidacion.show("Error... los datos ingresados no corresponde a un administrador.");
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void VentanaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            this.AddChild(inicio);
        }

        private void efectoPanel(UserControl panel)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0.0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            panel.BeginAnimation(UserControl.OpacityProperty, da);
        } 
    }
}
