using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SmarTravel_Final.Controller
{
    class TrayectoFacade
    {
        public static List<Trayecto> buscarPorRecorrido(int recorrido)
        {
            List<Trayecto> trayectos = new List<Trayecto>();
            if (recorrido > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select id, origen, destino, precio from trayecto where recorrido = " + recorrido;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        trayectos.Add(new Trayecto(dr.GetInt32(0), CiudadFacade.buscarPorId(dr.GetInt32(1)), CiudadFacade.buscarPorId(dr.GetInt32(2)), dr.GetInt32(3)));
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
            return trayectos;
        }

        public static Trayecto buscarPorId(int id)
        {
            Trayecto trayecto = null;
            if (id > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select id, origen, destino, precio from trayecto where id = " + id;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        trayecto = new Trayecto(dr.GetInt32(0), CiudadFacade.buscarPorId(dr.GetInt32(1)), CiudadFacade.buscarPorId(dr.GetInt32(2)), dr.GetInt32(3));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    trayecto = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return trayecto;
        }
        
        public static List<Trayecto> buscarPorRecorridoDestino(int recorrido, string destino)
        {
            List<Trayecto> trayectos = new List<Trayecto>(); ;
            if (destino != "" && recorrido > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    Ciudad d = CiudadFacade.buscarPorNombre(destino);
                    string sql = "select id, origen, destino, precio from trayecto where destino = " + d.id + " and recorrido = " + recorrido;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        trayectos.Add(new Trayecto(dr.GetInt32(0), CiudadFacade.buscarPorId(dr.GetInt32(1)), CiudadFacade.buscarPorId(dr.GetInt32(2)), dr.GetInt32(3)));
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
            return trayectos;
        }

        public static List<Trayecto> buscarOrigenes()
        {
            List<Trayecto> trayectos = new List<Trayecto>(); ;

            MySqlConnection con = conexionDB.ObtenerConexion();
            try
            {
                string sql = "select id, origen, destino, precio from trayecto group by origen";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    trayectos.Add(new Trayecto(dr.GetInt32(0), CiudadFacade.buscarPorId(dr.GetInt32(1)), CiudadFacade.buscarPorId(dr.GetInt32(2)), dr.GetInt32(3)));
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

            return trayectos;
        }

        public static List<Trayecto> buscarDestinosPorOrigen(string origen)
        {
            List<Trayecto> trayectos = new List<Trayecto>(); ;

            MySqlConnection con = conexionDB.ObtenerConexion();
            try
            {
                Ciudad o = CiudadFacade.buscarPorNombre(origen);
                string sql = "select id, origen, destino, precio from trayecto where origen = "+o.id+" group by destino";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    trayectos.Add(new Trayecto(dr.GetInt32(0), CiudadFacade.buscarPorId(dr.GetInt32(1)), CiudadFacade.buscarPorId(dr.GetInt32(2)), dr.GetInt32(3)));
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

            return trayectos;
        }

        public static Trayecto buscarPorOrigenDestinoRecorrido(string origen, string destino, int recorrido)
        {
            Trayecto trayecto = null;
            MySqlConnection con = conexionDB.ObtenerConexion();
            try
            {
                Ciudad cOrigen = CiudadFacade.buscarPorNombre(origen);
                Ciudad cDestino = CiudadFacade.buscarPorNombre(destino);

                string sql = "select id, precio from trayecto where origen=" + cOrigen.id + " and destino=" + cDestino.id + " and recorrido=" + recorrido;
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        trayecto = new Trayecto(dr.GetInt32(0), cOrigen, cDestino, dr.GetInt32(1));
                    }
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
            return trayecto;
        }

        public static void guardar(List<Trayecto> trayectos, int idRecorrido)
        {
            if (trayectos.Count > 0 && idRecorrido > -1)
            {                
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    MySqlCommand cmd;
                    foreach (Trayecto t in trayectos) 
                    {
                        cmd = new MySqlCommand(string.Format("INSERT INTO TRAYECTO (RECORRIDO, ORIGEN, DESTINO, PRECIO) VALUES ('{0}','{1}','{2}','{3}')", idRecorrido, t.origen.id, t.destino.id, t.precio), con);
                        cmd.ExecuteNonQuery();
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
        }
    }
}
