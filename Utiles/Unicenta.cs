using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Utiles
{
    public class Unicenta
    {

        #region Contructor
        public Unicenta()
        {
            Con_E = "Server=192.168.1.130;Database=unicentaopos;Uid=sa;Pwd=sa;";
        }
        public Unicenta(string conexion_mysql)
        {
            Con_E = conexion_mysql;
        }
        #endregion
        private string Con_E { get; set; }
        public bool sql_update(string sql)
        {
            // Properties.Settings.Default.Conex_Expert;

            using (MySqlConnection con = new MySqlConnection(Con_E))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public DataTable Sql_Datatable(string sql)
        {
            //Properties.Settings.Default.Conex_Expert;
            DataTable dt = new DataTable("Lineas");

            using (MySqlConnection cnn2 = new MySqlConnection(Con_E))
            {
                try
                {
                    cnn2.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = cnn2;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    dt.Load(dr);

                    cnn2.Dispose();
                    return dt;
                }
                catch (Exception)
                {
                    throw;

                }

            }
        }
    }
}
