using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class PersonaFacade
    {
        public static Persona buscarPorRut(string rut)
        {
            Persona persona = null;

            if (rut != "")
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select rut, nombre_completo, edad, direccion, ciudad, fono, clave, imagen, sexo, cargo from persona where rut = '"+rut+"'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        persona = new Persona(dr.GetString(0),dr.GetString(1), dr.GetInt32(2), dr.GetString(3), CiudadFacade.buscarPorId(dr.GetInt32(4)), dr.GetInt32(5), dr.GetString(6), dr.GetString(7), dr.GetString(8));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    persona = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return persona;
        }
    }
}
