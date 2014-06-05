using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SmarTravel_Final.Controller
{
    class ViajeDiarioFacade
    {     
        public static ViajeDiario buscarPorId(int id)
        {
            ViajeDiario viajeDiario = null;

            if (id > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select ID, TRAYECTO, FECHA, BUS, CHOFER, AUXILIAR, ASIENTOS_DISPONIBLES, RUTA_ARCHIVO FROM VIAJES_DIARIOS WHERE ID = " + id;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        viajeDiario = new ViajeDiario(dr.GetInt32(0), TrayectoFacade.buscarPorId(dr.GetInt32(1)), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetInt32(6));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    viajeDiario = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return viajeDiario;
        }

        public static List<ViajeDiario> buscarPorViaje(int idViaje)
        {
            List<ViajeDiario> viajeDiarios = new List<ViajeDiario>(); ;

            if (idViaje > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select ID, TRAYECTO, FECHA, BUS, CHOFER, AUXILIAR, ASIENTOS_DISPONIBLES, RUTA_ARCHIVO FROM VIAJES_DIARIOS WHERE VIAJE = " + idViaje;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        viajeDiarios.Add(new ViajeDiario(dr.GetInt32(0), TrayectoFacade.buscarPorId(dr.GetInt32(1)), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetInt32(6)));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return viajeDiarios;
        }
    }
}
