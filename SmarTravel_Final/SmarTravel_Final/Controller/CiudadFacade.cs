using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace SmarTravel_Final.Controller
{
    class CiudadFacade
    {
        public static List<Ciudad> buscarTodos()
        {
            List<Ciudad> ciudades = new List<Ciudad>();
            MySqlConnection con = conexionDB.ObtenerConexion();
            try
            {
                string sql = "SELECT C.ID, C.NOMBRE, C.REGION, C.NUMERO  FROM CIUDAD AS C INNER JOIN REGION AS R ON (C.NUMERO=R.ID) ORDER BY R.ID ASC";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ciudades.Add(new Ciudad(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetInt32(3)));
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ciudades = null;
            }
            finally
            {
                con.Close();
            }
            return ciudades;
        }




        public static Ciudad buscarPorId(int id)
        {
            Ciudad ciudad = null;
            if (id > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "SELECT C.ID, C.NOMBRE, C.REGION, C.NUMERO  FROM CIUDAD AS C INNER JOIN REGION AS R ON (C.NUMERO=R.ID) WHERE C.ID=" + id + " ORDER BY R.ID ASC";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ciudad = new Ciudad(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetInt32(3));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ciudad buscarPorId: "+ex.Message);
                    ciudad = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return ciudad;
        }

        public static Ciudad buscarPorNombre(string nombre)
        {
            Ciudad ciudad = null;
            if (nombre != "")
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "SELECT C.ID, C.NOMBRE, C.REGION, C.NUMERO  FROM CIUDAD AS C INNER JOIN REGION AS R ON (C.NUMERO=R.ID) WHERE C.NOMBRE LIKE '%" + nombre + "%' ORDER BY R.ID ASC";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ciudad = new Ciudad(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetInt32(3));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Ciudad buscarPorNombre: "+ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return ciudad;
        }

        public static Ciudad buscarCiudadPorNombre(string nombre)
        {
            Ciudad ciudad = null;
            if (nombre != "")
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "SELECT ID, NOMBRE, REGION, NUMERO FROM CIUDAD WHERE NOMBRE ='"+nombre+"'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ciudad = new Ciudad(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetInt32(3));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Ciudad buscarPorNombre: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return ciudad;
        }

        public static List<Ciudad> buscarPorRegion(string region)
        {
            List<Ciudad> ciudades = new List<Ciudad>();
            if (region != "")
            {                
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "SELECT C.ID, C.NOMBRE, C.REGION, C.NUMERO  FROM CIUDAD AS C INNER JOIN REGION AS R ON (C.NUMERO=R.ID) WHERE C.REGION ='" + region + "' ORDER BY R.ID ASC";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ciudades.Add(new Ciudad(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetInt32(3)));
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
            return ciudades;
        }
    }
}
