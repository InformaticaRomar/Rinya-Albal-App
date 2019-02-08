using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using OfficeOpenXml;

namespace Rinya_Albal_App.Logistica
{
    public partial class Faltas_pedidos_fechas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Btexport_Click(object sender, EventArgs e)
        {
            Get_Excel();
        }
        private string consulta_sql(string fecha1, string fecha2)
        {

            string sql = @"SELECT DISTINCT 
                      CONVERT(varchar, PEDIDO_CABE.[Fecha Pedido], 103) AS [Fecha Pedido], CONVERT(varchar, PEDIDO_CABE.[Fecha Entrega], 103) AS [Fecha Entrega], 
                      CLIENTE.Cliente, CLIENTE.[Nombre envio] AS [Nombre Cliente], PEDIDO_CABE.Empresa, PEDIDO_CABE.[Nº Pedido], PEDIDO_LIN.Artículo, ARTICULO.Descripción,ARTICULO.[Uds Venta], 
                      PEDIDO_LIN.[Cant Pedida en Uds] , 
                      PEDIDO_LIN.[Cant Recibida en Uds],PEDIDO_LIN.[Cant Pedida en Uds] -case when PEDIDO_LIN.[Cant Recibida en Uds] is not null then PEDIDO_LIN.[Cant Recibida en Uds]else '0' end as [Diferencia],
                      CLIENTE.[Cuadro comisiones] AS Comercial, [CUADRO COMISIONES].Descripción AS [nombre Comercial], PEDIDO_CABE.Observaciones
FROM         [CUADRO COMISIONES] INNER JOIN
                      CLIENTE ON [CUADRO COMISIONES].Cuadro = CLIENTE.[Cuadro comisiones] RIGHT OUTER JOIN
                      ARTICULO RIGHT OUTER JOIN
                      PEDIDO_LIN RIGHT OUTER JOIN
                      PEDIDO_CABE INNER JOIN
                      RUTA ON PEDIDO_CABE.Ruta = RUTA.Ruta ON PEDIDO_LIN.[Nº Pedido] = PEDIDO_CABE.[Nº Pedido] AND PEDIDO_LIN.Empresa = PEDIDO_CABE.Empresa AND 
                      PEDIDO_LIN.Año = PEDIDO_CABE.Año ON ARTICULO.Artículo = PEDIDO_LIN.Artículo ON CLIENTE.Cliente = PEDIDO_CABE.[Código C/P]
WHERE     (PEDIDO_CABE.[Fecha Entrega] >= CONVERT(datetime,'" + fecha1 + @"', 103)) AND (PEDIDO_CABE.[Fecha Entrega] <= CONVERT(datetime,'" + fecha2 + @"', 103))  and PEDIDO_CABE.Ruta between 99 and 160 and  Cliente is not null and (PEDIDO_LIN.[Cant Pedida en Uds]<>PEDIDO_LIN.[Cant Recibida en Uds] OR PEDIDO_LIN.[Cant Recibida en Uds]is null OR PEDIDO_LIN.[Cant Pedida en Uds] is null)";
            


            return sql;
        }
        private bool Get_Excel()
        {
            Quality con = new Quality();
            List<string> dateColumns = new List<string>() {

                    "Fecha Pedido",
                    "Fecha Entrega"
                };
            //string sql = consulta_sql(string agencia);
            List<string> hideColumns = new List<string>() {

                "orden"
            };
            using (ExcelPackage pck = new ExcelPackage())
            {


                DataTable table = con.Sql_Datatable(consulta_sql(datepicker1.Text, datepicker2.Text));
                if (table == null)//&& table.Rows.Count <= 0)
                {
                    System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('No hay precio 0 motivo 6')</SCRIPT>");
                }
                else
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                    ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                    FormatWorksheetData(dateColumns, hideColumns, table, ws);

                    // make sure it is sent as a XLSX file
                    Response.ContentType = "application/vnd.ms-excel";
                    // make sure it is downloaded rather than viewed in the browser window
                    Response.AddHeader("Content-disposition", "attachment; filename=integracion_agencia.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
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

    }
}