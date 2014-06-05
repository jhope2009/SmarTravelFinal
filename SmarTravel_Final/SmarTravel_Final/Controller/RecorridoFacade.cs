using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SmarTravel_Final.Controller
{
    class RecorridoFacade
    {
        public static Recorrido buscarPorId(int id)
        {
            Recorrido recorrido = null;
            if (id > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select id, parada from recorrido where id = " + id;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        List<Trayecto> trayectos = TrayectoFacade.buscarPorRecorrido(dr.GetInt32(0));
                        Console.WriteLine("trayectos buscarPorId: " + trayectos.Count);
                        recorrido = new Recorrido(dr.GetInt32(0), ParadaFacade.buscarPorId(dr.GetInt32(1)),trayectos);
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    recorrido = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return recorrido;
        }

        public static List<Recorrido> buscarPorOrigen(string origen)
        {
            List<Recorrido> recorridos = new List<Recorrido>();
            if (origen != "")
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    Ciudad o = CiudadFacade.buscarPorNombre(origen);
                    string sql = "select r.ID, r.PARADA from recorrido as r inner join trayecto as t on (r.ID = t.RECORRIDO) where t.ORIGEN="+o.id+" group by r.ID";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {                        
                        List<Trayecto> trayectos = TrayectoFacade.buscarPorRecorrido(dr.GetInt32(0));
                        Console.WriteLine("trayectos buscarPorOrigen: " + trayectos.Count);
                        recorridos.Add(new Recorrido(dr.GetInt32(0), ParadaFacade.buscarPorId(dr.GetInt32(1)), trayectos));                        
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
            return recorridos;
        }        

        public static List<Recorrido> buscarPorOrigenDestino(string origen, string destino)
        {
            List<Recorrido> recorridos = new List<Recorrido>();
            if (origen != "" && destino != "")
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    List<Recorrido> recs = RecorridoFacade.buscarPorOrigen(origen);
                    if (recs.Count > 0)
                    {
                        foreach (Recorrido rec in recs)
                        {                            
                            List<Trayecto> tray = TrayectoFacade.buscarPorRecorridoDestino(rec.id, destino);
                            if (tray.Count > 0)
                            {
                                recorridos.Add(rec);
                            }
                        }
                    }
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
            return recorridos;
        }
        
        public static void guardar(Recorrido recorrido)
        {
            try
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                MySqlCommand cmd;
                MySqlDataReader dr;
                                 
                cmd = new MySqlCommand(string.Format("SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'buses' AND TABLE_NAME = 'recorrido'"), con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {                    
                    recorrido.id = (int)dr.GetInt32(0);
                }
                dr.Close();
                cmd = new MySqlCommand(string.Format("INSERT INTO RECORRIDO (ID, PARADA) VALUES ('{0}','{1}')", recorrido.id, recorrido.parada.id), con);
                cmd.ExecuteNonQuery();

                ParadaFacade.guardar(recorrido.parada, recorrido.id);
                TrayectoFacade.guardar(recorrido.trayectos, recorrido.id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
