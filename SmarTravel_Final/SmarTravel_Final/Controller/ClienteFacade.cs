using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmarTravel_Final.Controller
{
    class ClienteFacade
    {
        public static Cliente buscarPorRut(string rut)
        {
            Cliente cliente = null;

            if (rut != "")
            {
                MySqlConnection con = conexionDB.ObtenerConexion();
                try
                {
                    string sql = "select rut, tarjeta, descuento from cliente where rut = '"+rut+"'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Persona persona = PersonaFacade.buscarPorRut(dr.GetString(0));
                        cliente = new Cliente(persona.rut, persona.nombre, persona.edad, persona.direccion, persona.ciudad, persona.fono, persona.clave, persona.sexo, persona.cargo, dr.GetInt32(1), dr.GetInt32(2));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    cliente = null;
                }
                finally
                {
                    con.Close();
                }
            }
            return cliente;
        }
    }
}
