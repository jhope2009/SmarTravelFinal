using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class PasajeFacade
    {
        public static Pasaje buscarPorNumero(int numero)
        {
            Pasaje pasaje = null;

            if (numero > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select numero, viaje_diario, precio, fecha_venta, asiento, estado from pasaje where numero = "+numero;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        pasaje = new Pasaje(dr.GetInt32(0), ViajeDiarioFacade.buscarPorId(dr.GetInt32(1)), dr.GetInt32(2), dr.GetString(3), dr.GetInt32(4), dr.GetString(5));
                    }
                    dr.Close();

                    sql = "select rut, numero from cliente_pasaje where numero = " + numero;
                    cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr1 = cmd.ExecuteReader();

                    while (dr1.Read())
                    {
                        pasaje.cliente = ClienteFacade.buscarPorRut(dr.GetString(0));
                    }
                    dr1.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    pasaje = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return pasaje;
        }

        public static void guardar(Pasaje pasaje)
        {
            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                MySqlCommand cmd;

                if (pasaje.cliente != null)
                {
                    int numero = -1;
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'buses' AND TABLE_NAME = 'pasaje'";
                    cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        numero = dr.GetInt32(0);
                    }
                    dr.Close();

                    cmd = new MySqlCommand(string.Format("INSERT INTO CLIENTE_PASAJE (RUT, NUMERO) VALUES ('{0}','{1}')", pasaje.cliente.rut, numero), con);
                    cmd.ExecuteNonQuery();
                }
                
                cmd = new MySqlCommand(string.Format("INSERT INTO PASAJE (VIAJE_DIARIO, PRECIO, FECHA_VENTA, ASIENTO, ESTADO) VALUES ('{0}','{1}','{2}','{3}','{4}')", pasaje.viajeDiario.id, pasaje.precio, pasaje.fechaVenta, pasaje.asiento, pasaje.estado), con);
                cmd.ExecuteNonQuery();                
            }
            catch (Exception ex)
            {
                Console.WriteLine("PasajeFacade guardar: "+ex.Message);
            }
        }  
    }
}
