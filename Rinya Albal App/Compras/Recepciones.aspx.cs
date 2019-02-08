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

namespace Rinya_Albal_App.Compras
{
    public partial class Recepciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string datos_compras(string F_desde, string F_hasta)
        {
            string sql = @"select PROVEEDOR.Proveedor,PROVEEDOR.Nombre,ARTICULO.Artículo,ARTICULO.Descripción, convert (varchar, [Fecha Recepción],103) as [Fecha Recepción]
, ARTICULO.[Control exist], RECEPCION_LIN.Almacén
,Sum( case when ARTICULO.[Control exist]= 'U' then RECEPCION_lin.Cajas* ARTICULO.[Factor(Kg/Ud)] else Cantidad end) as KG
,sum(case when RECEPCION_lin.Cajas>0 then RECEPCION_lin.Cajas else round (case when ARTICULO.[Uds Venta]= 'Ud' then Cantidad else Cantidad/ ARTICULO.[Factor(Kg/Ud)] end,0)end ) as UD

   from RECEPCION_cabe inner join recepcion_lin on RECEPCION_cabe.año=RECEPCION_lin.año
  and RECEPCION_cabe.empresa= RECEPCION_lin.empresa and RECEPCION_cabe.serie= RECEPCION_lin.serie and RECEPCION_cabe.[Nº Albarán]= RECEPCION_lin.[Nº albarán]
  inner join PROVEEDOR on PROVEEDOR.proveedor= RECEPCION_cabe.[Código proveedor]
  inner join ARTICULO on ARTICULO.Artículo= RECEPCION_lin.Artículo
  
    where[Fecha Recepción] between convert(datetime,'" + F_desde.Replace('-','/') + @"',103) and convert(datetime,'"+F_hasta.Replace('-', '/') + @"',103)
  group by  PROVEEDOR.Proveedor,PROVEEDOR.Nombre,ARTICULO.Artículo,ARTICULO.Descripción,  [Fecha Recepción]
,ARTICULO.[Control exist], RECEPCION_LIN.Almacén";
            return sql;
        }
        private bool Get_Excel()
        {
            Quality con = new Quality();
            string F_desde = datepicker1.Value;
            string F_hasta = datepicker2.Value;
            string sql = datos_compras(F_desde, F_hasta);
            List<string> hideColumns = new List<string>() {

                "orden"
            };

            using (ExcelPackage pck = new ExcelPackage())
            {


                DataTable table = con.Sql_Datatable(sql);
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                FormatWorksheetData(hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Recepciones.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            return true;
        }
        private static void FormatWorksheetData(List<string> dateColumns, List<string> hideColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            ExcelRange r;

            // which columns have dates in
            for (int i = 1; i <= columnCount; i++)
            {
                // if cell header value matches a date column
                if (dateColumns.Contains(ws.Cells[1, i].Value.ToString()))
                {
                    r = ws.Cells[2, i, rowCount + 1, i];
                    r.AutoFitColumns();
                    r.Style.Numberformat.Format = @"dd MMM yyyy hh:mm";
                }
            }
            // get all data and autofit
            r = ws.Cells[1, 1, rowCount + 1, columnCount];
            r.AutoFitColumns();

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