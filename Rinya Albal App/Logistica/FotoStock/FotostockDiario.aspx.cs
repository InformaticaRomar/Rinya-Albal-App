using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using CustomControls;
using System.Data.SqlClient;
using OfficeOpenXml;

namespace Rinya_Albal_App.Logistica.FotoStock
{
    public partial class FotostockDiario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string consulta_sql(string fecha1)
        {
            string sql = @"SELECT 
      [FOTO_STOCK].[Articulo]
      , REPLACE(REPLACE(ARTICULO.Descripción, CHAR(10), ''), CHAR(13), '') Descipcion
      ,[UdQuality]
      ,[KgAlternativo]
      ,[UdAlternativo]
      ,[KgQuality]
      ,[ControlExist]
      ,[Almacen],Familia
        FROM[QC_PRUEBAS].[dbo].[FOTO_STOCK]
        inner join ARTICULO on ARTICULO.Artículo=[FOTO_STOCK].Articulo
where[Fecha] = convert(datetime, '" + fecha1 +"', 103)";
            return sql;
        }

        private bool Get_Excel()
        {
            Quality con = new Quality();

            //string sql = consulta_sql(string agencia);
            List<string> hideColumns = new List<string>() {

                "orden"
            };
            using (ExcelPackage pck = new ExcelPackage())
            {
                string fecha= datepicker1.Value;

                DataTable table = con.Sql_Datatable(consulta_sql(fecha.Replace("-", @"/")));
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                FormatWorksheetData(hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Stock_Diario.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            return true;
        }
        private static void FormatWorksheetData(List<string> hideColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            // which columns have columns that should be hidden
            for (int i = 1; i <= columnCount; i++)
            {
                // if cell header value matches a hidden column
                if (hideColumns.Contains(ws.Cells[1, i].Value.ToString()))
                {
                    ws.Column(i).Hidden = true;
                }
            }
        }

        protected void Btexport_Click(object sender, EventArgs e)
        {
            Get_Excel();
        }
    }
}