using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SmarTravel_Final.Controller
{
    class ViajeFacade
    {
        public static Viaje buscarPorId(int id)
        {
            Viaje viaje = null;

            if (id > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select ID, RECORRIDO, IDENTIFICADOR, DESDE, HASTA from VIAJES WHERE ID= " + id;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        viaje = new Viaje(dr.GetInt32(0), dr.GetInt32(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), ViajeDiarioFacade.buscarPorViaje(id), HorarioFacade.buscarPorViaje(dr.GetInt32(0)));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Viaje buscarPorId: "+ex.Message);
                    viaje = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return viaje;
        }

        public static List<Viaje> buscarPorOrigenDestino(string origen, string destino)
        {
            List<Viaje> viajes = new List<Viaje>();

            if (origen != "" && destino != "")
            {
                List<Recorrido> recorridos = RecorridoFacade.buscarPorOrigenDestino(origen, destino);
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    foreach (Recorrido recorrido in recorridos)
                    {
                        string sql = "select ID, RECORRIDO, IDENTIFICADOR, DESDE, HASTA from VIAJES WHERE RECORRIDO= " + recorrido.id;
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        MySqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            viajes.Add(new Viaje(dr.GetInt32(0), dr.GetInt32(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), ViajeDiarioFacade.buscarPorViaje(dr.GetInt32(0)), HorarioFacade.buscarPorViaje(dr.GetInt32(0))));
                        }
                        dr.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("List<Viaje> buscarPorOrigenDestino: "+ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return viajes;
        }        
    }
}
