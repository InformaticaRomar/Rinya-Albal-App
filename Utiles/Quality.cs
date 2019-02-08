using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Utiles
{
    public class Quality
    {
        public DataTable Sql_Datatable(string sql)
        {
            DataTable dt = new DataTable("Lineas");
            try
            {
                string Con_Q = @"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso";
                // Properties.Settings.Default.Conex_Quality;

                using (SqlConnection cnn = new SqlConnection(Con_Q))
                {


                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
                    da.Fill(dt);
                    cnn.Close();
                    return dt;
                }
            }
            catch (Exception)
            {
                return null;// throw;
            }
           }

        public DataTable Sql_Datatable(string sql, string Con_Q)
        {
            DataTable dt = new DataTable("Lineas");
            try
            {
               // string Con_Q = @"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso";
                // Properties.Settings.Default.Conex_Quality;

                using (SqlConnection cnn = new SqlConnection(Con_Q))
                {


                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
                    da.Fill(dt);
                    cnn.Close();
                    return dt;
                }
            }
            catch (Exception)
            {
                return null;// throw;
            }
        }
        public DataTable Sql_Procedure_Datatable(string sql, string [,] parametros) {
            DataTable dt = new DataTable("Lineas");
            try
            {
                string Con_Q = @"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso";
                using (SqlConnection cnn = new SqlConnection(Con_Q))
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;

                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.StoredProcedure;
                    for ( int i =0; i< parametros.GetLength(0); i++) { 
                    cmd.Parameters.Add("@"+parametros[i,0].ToString(), SqlDbType.VarChar).Value = parametros[i, 1].ToString();
                    }
                    cmd.Connection = cnn;

                    cnn.Open();

                    reader = cmd.ExecuteReader();
                    // Data is accessible through the DataReader object here.
                    dt.Load(reader);
                    cnn.Close();
                    
                    
                    return dt;
                }
            }
            catch (Exception)
            {
                return null;// throw;
            }
           

        }
        public DataTable Sql_Procedure_Datatable(string sql)
        {
            DataTable dt = new DataTable("Lineas");
            try
            {
                string Con_Q = @"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso";
                using (SqlConnection cnn = new SqlConnection(Con_Q))
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;

                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Connection = cnn;

                    cnn.Open();

                    reader = cmd.ExecuteReader();
                    // Data is accessible through the DataReader object here.
                    dt.Load(reader);
                    cnn.Close();


                    return dt;
                }
            }
            catch (Exception)
            {
                return null;// throw;
            }


        }
        public string sql_string(string sql) {
            DataTable dt = new DataTable("Lineas");
            string result = "";
            try
            {
                string Con_Q = @"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso";
                // Properties.Settings.Default.Conex_Quality;

               

                    using (SqlConnection cnn = new SqlConnection(Con_Q))
                    {
                        SqlCommand command = new SqlCommand(sql, cnn);
                        command.Connection.Open();
                        //command.ExecuteNonQuery();
                    result = command.ExecuteScalar().ToString(); ;
                    command.Connection.Close();
                    
                    
                }
                   
              }
            
            catch (Exception)
            {
                
            }
            return result;
        }
        public bool sql_update(string sql)
        {

            try
            {
                string Con_Q = @"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso";
                // Properties.Settings.Default.Conex_Quality;

                using (SqlConnection cnn = new SqlConnection(Con_Q))
                {
                    SqlCommand command = new SqlCommand(sql, cnn);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                    
                }
            }
            catch (Exception)
            {
                return false;
            }
       
            return true;
        }
        public DataTable ArrayToDataTable(Array array, bool headerQ = true)
        {
            if (array == null || array.GetLength(1) == 0 || array.GetLength(0) == 0) return null;
            System.Data.DataTable dt = new System.Data.DataTable();
            int dataRowStart = headerQ ? 1 : 0;

            // create columns
            for (int i = 1; i <= array.GetLength(1); i++)
            {
                var column = new DataColumn();
                string value = array.GetValue(1, i) is System.String
                    ? array.GetValue(1, i).ToString() : "Column" + i.ToString();

                column.ColumnName = value;
                dt.Columns.Add(column);
            }
            if (array.GetLength(0) == dataRowStart) return dt;  //array has no data

            //Note:  the array is 1-indexed (not 0-indexed)
            for (int i = dataRowStart + 1; i <= array.GetLength(0); i++)
            {
                // create a DataRow using .NewRow()
                DataRow row = dt.NewRow();

                // iterate over all columns to fill the row
                for (int j = 1; j <= array.GetLength(1); j++)
                {
                    row[j - 1] = array.GetValue(i, j);
                }

                // add the current row to the DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }

    }
}
