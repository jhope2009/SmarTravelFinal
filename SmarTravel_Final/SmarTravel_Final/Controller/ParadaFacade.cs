using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

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

        public static List<Parada> buscarCiudadesPorRecorrido(int recorrido)
        {
            List<Parada> paradas = new List<Parada>();
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
                        paradas.Add(new Parada(dr.GetInt32(0), new Ciudad(dr.GetInt32(1), dr.GetString(2), dr.GetString(3), dr.GetInt32(4)), ParadaFacade.buscarPorId(dr.GetInt32(5))));
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
            return paradas;
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
        /*
        public static List<List<Parada>> buscarIntermediosByRecorrido(List<Recorrido> recorridos)
        {
            List<Parada> Intermedios = new List<Parada>();
            List< List<Parada>> Recorrido = new List<List<Parada>>(); 
            try
            {
                foreach(Recorrido reco in recorridos)
                {
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    string destinos = "SELECT P.ID,P.RECORRIDO,C.ID,C.NOMBRE,C.REGION,C.NUMERO,P.SIGUIENTE FROM PARADA AS P INNER JOIN CIUDAD AS C ON P.CIUDAD = C.ID WHERE P.RECORRIDO =" + reco.id;

                    MySqlCommand cmd = new MySqlCommand(destinos, con);

                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Ciudad Origen = new Ciudad(dr.GetInt32(2), dr.GetString(3), dr.GetString(4), dr.GetInt32(5));

                        Parada siguiente = ParadaFacade.buscarPorId(dr.GetInt32(6));
                        Intermedios.Add(new Parada(dr.GetInt32(0), dr.GetInt32(1), Origen, siguiente));

                    }
                    con.Close();
                    Recorrido.Add(Intermedios);
                }
            }
            catch (MySqlException ex)
            {
                validar v = new validar();
                v.show(ex.Message);
            }

            return Recorrido;
        }
         * */
        public static List<Parada> buscarDestinosGeneralesByCiudad(Ciudad origen, List<int> recorridoOrigenes)
        {
            List<Parada> allDestinosGenerales = new List<Parada>();
            try
            {
                for(int i=0; i<recorridoOrigenes.Count;i++)
                {
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    string destinos = "SELECT P.ID,P.RECORRIDO,C.ID,C.NOMBRE,C.REGION,C.NUMERO,P.SIGUIENTE FROM PARADA AS P INNER JOIN CIUDAD AS C ON P.CIUDAD = C.ID WHERE P.SIGUIENTE = -1 AND P.RECORRIDO =" + recorridoOrigenes[i];

                    MySqlCommand cmd = new MySqlCommand(destinos, con);

                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()){
                    Ciudad Origen = new Ciudad(dr.GetInt32(2), dr.GetString(3), dr.GetString(4), dr.GetInt32(5));

                    Parada siguiente = ParadaFacade.buscarPorId(dr.GetInt32(6));
                    allDestinosGenerales.Add(new Parada(dr.GetInt32(0), dr.GetInt32(1), Origen, siguiente));
                }
            }
            }
            catch (MySqlException ex) {
                validar v = new validar();
                v.show(ex.Message);
            }
        
            return allDestinosGenerales;
        }

        public static string nombresIntermediosByRecorridos(int reco)
        {

            MySqlConnection con = conexionDB.ObtenerConexion();
            string sqlRecorridos = "SELECT NOMBRE FROM PARADA INNER JOIN CIUDAD ON PARADA.CIUDAD = CIUDAD.ID WHERE RECORRIDO ="+reco;
            MySqlCommand cmd = new MySqlCommand(sqlRecorridos, con);

            string nombres = "";
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                nombres = nombres + dr.GetString(0) + " - ";

            }

            return nombres;
        }
        public static List<Parada> buscarOrigenesGenerales() {

            List<Parada> allOrigenesGenerales = new List<Parada>();
            int numeroRecorridos = RecorridoFacade.totalRecorridos();
            // RECORRO LOS RECORRIDOS GENERALES.

            try
            {
                int i;
                for (i = 0; i <= numeroRecorridos; i++)
                {
                    MySqlConnection con = conexionDB.ObtenerConexion();
                    string sqlRecorridos = "SELECT P.ID,P.RECORRIDO,C.ID,C.NOMBRE,C.REGION,C.NUMERO,P.SIGUIENTE FROM PARADA AS P INNER JOIN CIUDAD AS C ON P.CIUDAD = C.ID WHERE RECORRIDO = '" + i + "' LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(sqlRecorridos, con);

                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Ciudad Origen = new Ciudad(dr.GetInt32(2), dr.GetString(3), dr.GetString(4), dr.GetInt32(5));

                        Parada siguiente = ParadaFacade.buscarPorId(dr.GetInt32(6));
                        allOrigenesGenerales.Add(new Parada(dr.GetInt32(0), dr.GetInt32(1), Origen, siguiente));
                        //id_recorrido.Add(i);
                    }
                    con.Close();
                }
            }
            catch (MySqlException ex)
            {
                validar v = new validar();
                v.show(ex.Message);
            }
            return allOrigenesGenerales;
        }

    }
}
