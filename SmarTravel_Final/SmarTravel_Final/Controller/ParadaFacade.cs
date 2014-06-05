using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SmarTravel_Final.Controller
{
    class ParadaFacade
    {
        public static Parada buscarPorRecorrido(int recorrido)
        {
            Parada parada = new Parada();
            if (recorrido > -1)
            {                
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select p.ID, c.ID, c.NOMBRE, c.REGION, c.NUMERO, p.SIGUIENTE from parada as p inner join ciudad as c on (p.CIUDAD=c.ID) where recorrido =" + recorrido.ToString();
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        parada = new Parada(dr.GetInt32(0), new Ciudad(dr.GetInt32(1), dr.GetString(2), dr.GetString(3), dr.GetInt32(4)), ParadaFacade.buscarPorId(dr.GetInt32(5)));
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
            return parada;
        }

        public static Parada buscarPorId(int id)
        {
            Parada parada = new Parada();
            Console.WriteLine("id : " + id);
            if (id > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select p.ID, c.ID, c.NOMBRE, c.REGION, c.NUMERO, p.SIGUIENTE from parada as p inner join ciudad as c on (p.CIUDAD=c.ID) where p.id =" + id.ToString();
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    
                    while (dr.Read())
                    {
                        Console.WriteLine("parada " + dr.GetInt32(0) + ": siguiente" + dr.GetInt32(5));
                        parada = new Parada(dr.GetInt32(0), new Ciudad(dr.GetInt32(1), dr.GetString(2), dr.GetString(3), dr.GetInt32(4)), ParadaFacade.buscarPorId(dr.GetInt32(5)));                                                
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
            return parada;
        }

        public static Parada buscarPorRecorridoCiudad(int recorrido, string ciudad)
        {
            Parada parada = new Parada();
            if (ciudad != "" && recorrido > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select p.ID, c.ID, c.NOMBRE, c.REGION, c.NUMERO, p.SIGUIENTE from parada as p inner join ciudad as c on (p.CIUDAD=c.ID) where c.NOMBRE LIKE '%"+ciudad+"%' and recorrido = "+recorrido;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        parada = new Parada(dr.GetInt32(0), new Ciudad(dr.GetInt32(1), dr.GetString(2), dr.GetString(3), dr.GetInt32(4)), ParadaFacade.buscarPorId(dr.GetInt32(5)));
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
            return parada;
        }

        public static void guardar(Parada parada, int idRecorrido)
        {
            if (parada != null && idRecorrido > -1)
            {
                Parada p = parada;
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {                   
                    MySqlCommand cmd;                                        
                    while (p.id > -1)
                    {
                        cmd = new MySqlCommand(string.Format("INSERT INTO PARADA (RECORRIDO, CIUDAD, SIGUIENTE) VALUES ('{0}','{1}','{2}')", idRecorrido, p.ciudad.id, p.siguiente.id), con);
                        cmd.ExecuteNonQuery();
                        p = p.siguiente;
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

        public static int obtenerProximoId()
        {
            int id = -1;
            MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "SELECT `AUTO_INCREMENT` FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'buses' AND TABLE_NAME = 'parada'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        id = dr.GetInt32(0);
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
            return id;
        }

    }
}
