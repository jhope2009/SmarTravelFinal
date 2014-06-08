using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class HorarioFacade
    {
        public static Horario buscarPorId(int id)
        {
            Horario horario = null;

            if (id > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select id, salida, llegada, parada from horarios where id = " + id;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        horario = new Horario(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), ParadaFacade.buscarPorId(dr.GetInt32(3)));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    horario = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return horario;
        }

        public static List<Horario> buscarPorViaje(int idViaje)
        {
            List<Horario> horarios = new List<Horario>();

            if (idViaje > -1)
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select id, salida, llegada, parada from horarios where viaje = " + idViaje;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        horarios.Add(new Horario(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), ParadaFacade.buscarPorId(dr.GetInt32(3))));
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
            return horarios;
        }
    }
}
