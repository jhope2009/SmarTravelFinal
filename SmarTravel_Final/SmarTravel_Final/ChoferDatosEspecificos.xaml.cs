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
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data.Sql;
using System.Data;
using System.IO;


namespace SmarTravel_Final
{
    /// <summary>
    /// Lógica de interacción para ChoferDatosEspecificos.xaml
    /// </summary>
    public partial class ChoferDatosEspecificos : Window
    {
        string rutaimglicencia = "";
        public ChoferDatosEspecificos()
        {
            InitializeComponent();
        }

        private void btnGenerarContrato_Click(object sender, RoutedEventArgs e)
        {
            validar validaimg = new validar();
            if (rutaimglicencia == "")
                validaimg.show("No ha ingresado imagen de su licencia de conducir");
            else if (panelUsuario.validar1(dateVencimientoLicencia.Text, txtVencimientoLicencia.Text, txtBNumerolicencia.Text, txtBNumerolicencia.Text))
            {
                String contratourl = generarContrato("Temuco");
                
                try
                {
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    String rutchofer = txtBRutChofer.Text+"-"+txtBdigVerificadorChofer.Text;
                    

                    string @path = System.IO.Directory.GetCurrentDirectory();
                    path = path.Substring(0, path.Length - 9);
                    path = path + "Images/fotoPerfil/";
                    string filePath = path + System.IO.Path.GetFileName(rutaimglicencia);

                    System.IO.File.Copy(rutaimglicencia, filePath, true);
                    string sql = "INSERT INTO CHOFER (persona, numero_licencia, vencimiento_licencia, contrato, imagen_licencia) VALUES ";
                    sql += "(?persona,?numerolicencia,?vencimientolicencia,?contrato,?imagenl)";

                    MySqlCommand insertCommand = new MySqlCommand(sql, con);

                    insertCommand.Parameters.Add("?persona", rutchofer);
                    insertCommand.Parameters.Add("?numerolicencia", txtBNumerolicencia);
                    insertCommand.Parameters.Add("?vencimientolicencia", dateVencimientoLicencia.Text);
                    insertCommand.Parameters.Add("?contrato", contratourl.ToString());
                    insertCommand.Parameters.Add("?imagenl", filePath.ToString());

                    insertCommand.ExecuteNonQuery();
                    con.Close();
                    nuevoUsuario mensajeNuevo = new nuevoUsuario();
                    mensajeNuevo.show(txtBNombre.Text);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                this.Close();
            }
            
        }
        private string generarContrato(string ciudad) 
        {
            DateTime today = DateTime.Today;
            string[] textocontrato = new string[12];
            textocontrato[0]= ciudad+", " + today.ToString(("d"));
            textocontrato[1] = "Oficina SmarTravel";
            textocontrato[2] = "Rudecindo Ortega 02950 salida norte, Temuco";
            textocontrato[3] = "Representante legal Felipe Lagos Morapastene";

            textocontrato[4] = "Se remite el presente contrato con efectos de  solicitar,  que  se  autorice  ";
            textocontrato[5] = "la  suscripción  del  contrato  de  ejecución  de  servicios  que  adjunto  se  acompaña. ";
            textocontrato[6] = "El   cual   ya  ha   sido   rubricado  por   el   interesado  a   realizarse  en  base  a  los ";
            textocontrato[7] = "siguientes  datos:";
            textocontrato[8] = "a)Personal  contratado :  Sr(a) " + txtBNombre.Text + "   Rut :  " + txtBRutChofer.Text + "-" + txtBdigVerificadorChofer.Text;
            textocontrato[9] = "b)Funciones y o tareas a desempeñar: Chofer de buses urbanos y de encomienda";
            textocontrato[10] = "c)Remuneraciones: XXX.XXX.XXX";
            textocontrato[11] = "Duración del contrato:Indefinido";

            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = "";
            PdfPage pdfPage = pdf.AddPage();
            pdfPage.Orientation = PdfSharp.PageOrientation.Landscape;
            pdfPage.Size = PdfSharp.PageSize.B5; 
            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XImage image = XImage.FromFile("Timbre.jpg");
            graph.DrawImage(image, 300, 350, 116, 118);
            
            XFont font = new XFont("Verdana", 12);
            graph.DrawString(textocontrato[0], font, XBrushes.Black, new XRect(540, 40, 10, 0), XStringFormats.Default);
            graph.DrawString(textocontrato[1], font, XBrushes.Black, new XRect(40, 100, 10, 0), XStringFormats.Default); //fecha e inicio de pagina
            graph.DrawString(textocontrato[2], font, XBrushes.Black, new XRect(40, 115, 10, 0), XStringFormats.Default);
            graph.DrawString(textocontrato[3], font, XBrushes.Black, new XRect(40, 130, 10, 0), XStringFormats.Default);
            int rango = 200;
            int ejey = 170;
            for(int i=4;i<12;i++)
            {
                
                graph.DrawString(textocontrato[i], font, XBrushes.Black, new XRect(ejey, rango, 10, 0), XStringFormats.Default); //contenido del contrato
                rango += 18;
                ejey = 100;
            }
            string pdfFilename = txtBRutChofer.Text + "-" + txtBdigVerificadorChofer.Text + ".pdf" ;
            string path = @"C:\Contratos\";
            pdf.Save(path + pdfFilename);

            Process.Start(path + pdfFilename);
            return path + pdfFilename;
        }

        private void btnCargarLicencia_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Filter = "Archivos jpg(*.jpg)|*.jpg";
            open.Title = "Archivos Imagenes";
       
            if (open.ShowDialog() == true)
            {
                rutaimglicencia = open.FileName;
            }
            if (rutaimglicencia != "")
            {
                var uri = new Uri(rutaimglicencia);
                var bitmap = new BitmapImage(uri);
                imgLicenciaConducir.Source = bitmap;
            }
        }
        

    }
}
