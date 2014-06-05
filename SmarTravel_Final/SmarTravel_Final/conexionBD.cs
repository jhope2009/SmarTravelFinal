using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
namespace SmarTravel_Final
{
    public class conexionDB{

        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=buses; Uid=root; pwd=admin;");

            conectar.Open();
            return conectar;
        }
    }
}
